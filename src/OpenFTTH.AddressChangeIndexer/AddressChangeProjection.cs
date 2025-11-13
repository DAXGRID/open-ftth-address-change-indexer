using OpenFTTH.Core.Address;
using OpenFTTH.Core.Address.Events;
using OpenFTTH.EventSourcing;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace OpenFTTH.AddressChangeIndexer;

internal sealed record AccessAddress
{
    public string MunicipalCode { get; init; }
    public AccessAddressStatus Status { get; init; }
    public string RoadCode { get; init; }
    public string HouseNumber { get; init; }
    public double EastCoordinate { get; init; }
    public double NorthCoordinate { get; init; }
    public string? SupplementaryTownName { get; init; }
    public string? PlotId { get; init; }
    public Guid RoadId { get; init; }
    public Guid PostCodeId { get; init; }

    public AccessAddress(
        string municipalCode,
        AccessAddressStatus status,
        string roadCode,
        string houseNumber,
        double eastCoordinate,
        double northCoordinate,
        string? townName,
        string? plotId,
        Guid roadId,
        Guid postCodeId)
    {
        MunicipalCode = municipalCode;
        Status = status;
        RoadCode = roadCode;
        HouseNumber = houseNumber;
        EastCoordinate = eastCoordinate;
        NorthCoordinate = northCoordinate;
        SupplementaryTownName = townName;
        PlotId = plotId;
        RoadId = roadId;
        PostCodeId = postCodeId;
    }
}

internal sealed record UnitAddress
{
    public Guid AccessAddressId { get; init; }
    public UnitAddressStatus Status { get; init; }
    public string? FloorName { get; init; }
    public string? SuiteName { get; init; }
    public DateTime? ExternalUpdatedDate { get; init; }

    public UnitAddress(
        Guid accessAddressId,
        UnitAddressStatus status,
        string? floorName,
        string? suiteName,
        DateTime? externalUpdatedDate)
    {
        AccessAddressId = accessAddressId;
        Status = status;
        FloorName = floorName;
        SuiteName = suiteName;
        ExternalUpdatedDate = externalUpdatedDate;
    }
}

internal sealed class AddressChangeProjection : ProjectionBase
{
    private readonly ILogger<AddressChangeProjection> _logger;

    public ChannelReader<AddressChange> AddressChanges => _addressChangesChannel.Reader;

    private readonly Channel<AddressChange> _addressChangesChannel = Channel.CreateUnbounded<AddressChange>();
    private readonly Dictionary<Guid, UnitAddress> _unitAddressIdToUnitAddress = new();
    private readonly Dictionary<Guid, AccessAddress> _accessAddressIdToAccessAddress = new();
    private readonly Dictionary<Guid, List<Guid>> _accessAddressIdToUnitAddressIds = new();
    private readonly Dictionary<Guid, string> _roadIdToRoadName = new();
    private readonly Dictionary<Guid, List<Guid>> _roadIdToAccessAddressIds = new();

    public AddressChangeProjection(ILogger<AddressChangeProjection> logger)
    {
        _logger = logger;

        ProjectEventAsync<RoadCreated>(ProjectAsync);
        ProjectEventAsync<RoadNameChanged>(ProjectAsync);
        ProjectEventAsync<RoadDeleted>(ProjectAsync);

        ProjectEventAsync<AccessAddressCreated>(ProjectAsync);
        ProjectEventAsync<AccessAddressMunicipalCodeChanged>(ProjectAsync);
        ProjectEventAsync<AccessAddressStatusChanged>(ProjectAsync);
        ProjectEventAsync<AccessAddressRoadCodeChanged>(ProjectAsync);
        ProjectEventAsync<AccessAddressHouseNumberChanged>(ProjectAsync);
        ProjectEventAsync<AccessAddressSupplementaryTownNameChanged>(ProjectAsync);
        ProjectEventAsync<AccessAddressPlotIdChanged>(ProjectAsync);
        ProjectEventAsync<AccessAddressRoadIdChanged>(ProjectAsync);
        ProjectEventAsync<AccessAddressCoordinateChanged>(ProjectAsync);
        ProjectEventAsync<AccessAddressDeleted>(ProjectAsync);

        ProjectEventAsync<UnitAddressCreated>(ProjectAsync);
        ProjectEventAsync<UnitAddressAccessAddressIdChanged>(ProjectAsync);
        ProjectEventAsync<UnitAddressStatusChanged>(ProjectAsync);
        ProjectEventAsync<UnitAddressFloorNameChanged>(ProjectAsync);
        ProjectEventAsync<UnitAddressSuiteNameChanged>(ProjectAsync);
        ProjectEventAsync<UnitAddressDeleted>(ProjectAsync);
    }

    private async Task ProjectAsync(IEventEnvelope eventEnvelope)
    {
        try
        {
            switch (eventEnvelope.Data)
            {
                case (RoadCreated roadCreated):
                    HandleRoadCreated(roadCreated);
                    break;
                case (RoadNameChanged roadNameChanged):
                    await HandleRoadNameChanged(
                        roadNameChanged,
                        eventEnvelope.EventId,
                        eventEnvelope.GlobalVersion,
                        eventEnvelope.EventTimestamp
                    ).ConfigureAwait(false);
                    break;
                case (RoadDeleted roadDeleted):
                    HandleRoadDeleted(roadDeleted);
                    break;

                case (AccessAddressCreated accessAddressCreated):
                    HandleAccessAddressCreated(accessAddressCreated);
                    break;
                case (AccessAddressMunicipalCodeChanged accessAddressMunicipalCodeChanged):
                    await HandleAccessAddressMunicipalCodeChanged(
                        accessAddressMunicipalCodeChanged,
                        eventEnvelope.EventId,
                        eventEnvelope.GlobalVersion,
                        eventEnvelope.EventTimestamp
                    ).ConfigureAwait(false);
                    break;
                case (AccessAddressStatusChanged accessAddressStatusChanged):
                    await HandleAccessAddressStatusChanged(
                        accessAddressStatusChanged,
                        eventEnvelope.EventId,
                        eventEnvelope.GlobalVersion,
                        eventEnvelope.EventTimestamp
                    ).ConfigureAwait(false);
                    break;
                case (AccessAddressRoadCodeChanged accessAddressRoadCodeChanged):
                    await HandleAccessAddressRoadCodeChanged(
                        accessAddressRoadCodeChanged,
                        eventEnvelope.EventId,
                        eventEnvelope.GlobalVersion,
                        eventEnvelope.EventTimestamp
                    ).ConfigureAwait(false);
                    break;
                case (AccessAddressHouseNumberChanged accessAddressHouseNumberChanged):
                    await HandleAccessAddressHouseNumberChanged(
                        accessAddressHouseNumberChanged,
                        eventEnvelope.EventId,
                        eventEnvelope.GlobalVersion,
                        eventEnvelope.EventTimestamp
                    ).ConfigureAwait(false);
                    break;
                case (AccessAddressCoordinateChanged accessAddressCoordinateChanged):
                    await HandleAccessAddressCoordinateChanged(
                        accessAddressCoordinateChanged,
                        eventEnvelope.EventId,
                        eventEnvelope.GlobalVersion,
                        eventEnvelope.EventTimestamp
                    ).ConfigureAwait(false);
                    break;
                case (AccessAddressSupplementaryTownNameChanged accessAddressSupplementaryTownNameChanged):
                    await HandleAccessAddressSupplementaryTownNameChanged(
                        accessAddressSupplementaryTownNameChanged,
                        eventEnvelope.EventId,
                        eventEnvelope.GlobalVersion,
                        eventEnvelope.EventTimestamp
                    ).ConfigureAwait(false);
                    break;
                case (AccessAddressPlotIdChanged handleAccessAddressPlotIdChanged):
                    await HandleAccessAddressPlotIdChanged(
                        handleAccessAddressPlotIdChanged,
                        eventEnvelope.EventId,
                        eventEnvelope.GlobalVersion,
                        eventEnvelope.EventTimestamp
                    ).ConfigureAwait(false);
                    break;
                case (AccessAddressRoadIdChanged handleAccessAddressRoadIdChanged):
                    await HandleAccessAddressRoadIdChanged(
                        handleAccessAddressRoadIdChanged,
                        eventEnvelope.EventId,
                        eventEnvelope.GlobalVersion,
                        eventEnvelope.EventTimestamp
                    ).ConfigureAwait(false);
                    break;
                case (AccessAddressDeleted accessAddressDeleted):
                    await HandleAccessAddressDeleted(
                        accessAddressDeleted,
                        eventEnvelope.EventId,
                        eventEnvelope.GlobalVersion,
                        eventEnvelope.EventTimestamp
                    ).ConfigureAwait(false);
                    break;

                case UnitAddressCreated unitAddressCreated:
                    await HandleUnitAddressCreated(
                        unitAddressCreated,
                        eventEnvelope.EventId,
                        eventEnvelope.GlobalVersion,
                        eventEnvelope.EventTimestamp
                    ).ConfigureAwait(false);
                    break;
                case UnitAddressAccessAddressIdChanged unitAddressAccessAddressIdChanged:
                    await HandleUnitAddressAccessAddressIdChanged(
                        unitAddressAccessAddressIdChanged,
                        eventEnvelope.EventId,
                        eventEnvelope.GlobalVersion,
                        eventEnvelope.EventTimestamp
                    ).ConfigureAwait(false);
                    break;
                case UnitAddressStatusChanged unitAddressStatusChanged:
                    await HandleUnitAddressStatusChanged(
                        unitAddressStatusChanged,
                        eventEnvelope.EventId,
                        eventEnvelope.GlobalVersion,
                        eventEnvelope.EventTimestamp
                    ).ConfigureAwait(false);
                    break;
                case UnitAddressFloorNameChanged unitAddressFloorNameChanged:
                    await HandleUnitAddressFloorNameChanged(
                        unitAddressFloorNameChanged,
                        eventEnvelope.EventId,
                        eventEnvelope.GlobalVersion,
                        eventEnvelope.EventTimestamp
                    ).ConfigureAwait(false);
                    break;
                case UnitAddressSuiteNameChanged unitAddressSuiteNameChanged:
                    await HandleUnitAddressSuiteNameChanged(
                        unitAddressSuiteNameChanged,
                        eventEnvelope.EventId,
                        eventEnvelope.GlobalVersion,
                        eventEnvelope.EventTimestamp
                    ).ConfigureAwait(false);
                    break;
                case UnitAddressDeleted unitAddressDeleted:
                    await HandleUnitAddressDeleted(
                        unitAddressDeleted,
                        eventEnvelope.EventId,
                        eventEnvelope.GlobalVersion,
                        eventEnvelope.EventTimestamp
                    ).ConfigureAwait(false);
                    break;

                default:
                    throw new ArgumentException(
                        $"Could not handle typeof '{eventEnvelope.Data.GetType().Name}'");
            }
        }
        catch (Exception ex)
        {
            _logger.LogCritical("{Exception}, {EventEnvelope}", ex, JsonSerializer.Serialize(eventEnvelope));
            throw;
        }
    }

    private void HandleRoadCreated(RoadCreated roadCreated)
    {
        _roadIdToRoadName.Add(roadCreated.Id, roadCreated.Name);
        _roadIdToAccessAddressIds.Add(roadCreated.Id, new());
    }

    private async Task HandleRoadNameChanged(
        RoadNameChanged roadNameChanged,
        Guid eventId,
        long sequenceNumber,
        DateTime eventTimestamp)
    {
        var roadNameBefore = _roadIdToRoadName[roadNameChanged.Id];
        var roadNameAfter = roadNameChanged.Name;

        foreach (var accessAddressId in _roadIdToAccessAddressIds[roadNameChanged.Id])
        {
            foreach (var unitAddressId in _accessAddressIdToUnitAddressIds[accessAddressId])
            {
                await _addressChangesChannel.Writer.WriteAsync(
                    AccessAddressChangeConvert.RoadNameChanged(
                        unitAddressId: unitAddressId,
                        eventId: eventId,
                        externalUpdated: roadNameChanged.ExternalUpdatedDate,
                        sequenceNumber: sequenceNumber,
                        eventTimestamp: eventTimestamp,
                        roadNameBefore: roadNameBefore,
                        roadNameAfter: roadNameAfter))
                    .ConfigureAwait(false);
            }
        }

        _roadIdToRoadName[roadNameChanged.Id] = roadNameAfter;
    }

    private void HandleRoadDeleted(RoadDeleted roadDeleted)
    {
        if (_roadIdToAccessAddressIds[roadDeleted.Id].Count == 0)
        {
            _roadIdToRoadName.Remove(roadDeleted.Id);
            _roadIdToAccessAddressIds.Remove(roadDeleted.Id);
        }
        // This can happen if DAWA unit addresses points at deleted roads.
        else
        {
            //
            _logger.LogWarning(
                "Could not remove the road with '{RoadId}' because the following unit addresses still points to it {UnitAddresses}, {ExternalUpdatedDate}",
                roadDeleted.Id,
                string.Join(", ", _roadIdToAccessAddressIds[roadDeleted.Id]),
                roadDeleted.ExternalUpdatedDate
            );
        }
    }

    private void HandleAccessAddressCreated(AccessAddressCreated accessAddressCreated)
    {
        _accessAddressIdToUnitAddressIds.Add(accessAddressCreated.Id, new());

        _roadIdToAccessAddressIds[accessAddressCreated.RoadId].Add(accessAddressCreated.Id);

        _accessAddressIdToAccessAddress.Add(
            accessAddressCreated.Id,
            new(municipalCode: accessAddressCreated.MunicipalCode,
                status: accessAddressCreated.Status,
                roadCode: accessAddressCreated.RoadCode,
                houseNumber: accessAddressCreated.HouseNumber,
                northCoordinate: accessAddressCreated.NorthCoordinate,
                eastCoordinate: accessAddressCreated.EastCoordinate,
                townName: accessAddressCreated.TownName,
                plotId: accessAddressCreated.PlotId,
                roadId: accessAddressCreated.RoadId,
                postCodeId: accessAddressCreated.PostCodeId));
    }

    private async Task HandleAccessAddressMunicipalCodeChanged(
        AccessAddressMunicipalCodeChanged changedEvent,
        Guid eventId,
        long sequenceNumber,
        DateTime eventTimestamp)
    {
        var oldAccessAddress = _accessAddressIdToAccessAddress[changedEvent.Id];

        foreach (var unitAddressId in _accessAddressIdToUnitAddressIds[changedEvent.Id])
        {
            await _addressChangesChannel.Writer.WriteAsync(
                AccessAddressChangeConvert.MunicipalCodeChanged(
                    unitAddressId: unitAddressId,
                    eventId: eventId,
                    externalUpdated: changedEvent.ExternalUpdatedDate,
                    sequenceNumber: sequenceNumber,
                    eventTimestamp: eventTimestamp,
                    municipalCodeBefore: oldAccessAddress.MunicipalCode,
                    municipalCodeAfter: changedEvent.MunicipalCode))
                .ConfigureAwait(false);
        }

        _accessAddressIdToAccessAddress[changedEvent.Id] = oldAccessAddress with
        {
            MunicipalCode = changedEvent.MunicipalCode,
        };
    }

    private async Task HandleAccessAddressStatusChanged(
        AccessAddressStatusChanged changedEvent,
        Guid eventId,
        long sequenceNumber,
        DateTime eventTimestamp)
    {
        var oldAccessAddress = _accessAddressIdToAccessAddress[changedEvent.Id];

        foreach (var unitAddressId in _accessAddressIdToUnitAddressIds[changedEvent.Id])
        {
            await _addressChangesChannel.Writer.WriteAsync(
                AccessAddressChangeConvert.StatusChanged(
                    unitAddressId: unitAddressId,
                    eventId: eventId,
                    externalUpdated: changedEvent.ExternalUpdatedDate,
                    sequenceNumber: sequenceNumber,
                    eventTimestamp: eventTimestamp,
                    statusBefore: oldAccessAddress.Status,
                    statusAfter: changedEvent.Status))
                .ConfigureAwait(false);
        }

        _accessAddressIdToAccessAddress[changedEvent.Id] = oldAccessAddress with
        {
            Status = changedEvent.Status
        };
    }

    private async Task HandleAccessAddressRoadCodeChanged(
        AccessAddressRoadCodeChanged changedEvent,
        Guid eventId,
        long sequenceNumber,
        DateTime eventTimestamp)
    {
        var oldAccessAddress = _accessAddressIdToAccessAddress[changedEvent.Id];

        foreach (var unitAddressId in _accessAddressIdToUnitAddressIds[changedEvent.Id])
        {
            await _addressChangesChannel.Writer.WriteAsync(
                AccessAddressChangeConvert.RoadCodeChanged(
                    unitAddressId: unitAddressId,
                    eventId: eventId,
                    externalUpdated: changedEvent.ExternalUpdatedDate,
                    sequenceNumber: sequenceNumber,
                    eventTimestamp: eventTimestamp,
                    roadCodeBefore: oldAccessAddress.RoadCode,
                    roadCodeAfter: changedEvent.RoadCode))
                .ConfigureAwait(false);
        }

        _accessAddressIdToAccessAddress[changedEvent.Id] = oldAccessAddress with
        {
            RoadCode = changedEvent.RoadCode,
        };
    }

    private async Task HandleAccessAddressHouseNumberChanged(
        AccessAddressHouseNumberChanged changedEvent,
        Guid eventId,
        long sequenceNumber,
        DateTime eventTimestamp)
    {
        var oldAccessAddress = _accessAddressIdToAccessAddress[changedEvent.Id];

        foreach (var unitAddressId in _accessAddressIdToUnitAddressIds[changedEvent.Id])
        {
            await _addressChangesChannel.Writer.WriteAsync(
                AccessAddressChangeConvert.HouseNumberChanged(
                    unitAddressId: unitAddressId,
                    eventId: eventId,
                    externalUpdated: changedEvent.ExternalUpdatedDate,
                    sequenceNumber: sequenceNumber,
                    eventTimestamp: eventTimestamp,
                    houseNumberBefore: oldAccessAddress.HouseNumber,
                    houseNumberAfter: changedEvent.HouseNumber))
                .ConfigureAwait(false);
        }

        _accessAddressIdToAccessAddress[changedEvent.Id] = oldAccessAddress with
        {
            HouseNumber = changedEvent.HouseNumber,
        };
    }

    private async Task HandleAccessAddressCoordinateChanged(
        AccessAddressCoordinateChanged changedEvent,
        Guid eventId,
        long sequenceNumber,
        DateTime eventTimestamp)
    {
        var oldAccessAddress = _accessAddressIdToAccessAddress[changedEvent.Id];

        foreach (var unitAddressId in _accessAddressIdToUnitAddressIds[changedEvent.Id])
        {
            await _addressChangesChannel.Writer.WriteAsync(
                AccessAddressChangeConvert.CoordinateChanged(
                    unitAddressId: unitAddressId,
                    eventId: eventId,
                    externalUpdated: changedEvent.ExternalUpdatedDate,
                    sequenceNumber: sequenceNumber,
                    eventTimestamp: eventTimestamp,
                    eastCoordinateBefore: oldAccessAddress.EastCoordinate,
                    northCoordinateBefore: oldAccessAddress.NorthCoordinate,
                    eastCoordinateAfter: changedEvent.EastCoordinate,
                    northCoordinateAfter: changedEvent.NorthCoordinate))
                .ConfigureAwait(false);
        }

        _accessAddressIdToAccessAddress[changedEvent.Id] = oldAccessAddress with
        {
            EastCoordinate = changedEvent.EastCoordinate,
            NorthCoordinate = changedEvent.NorthCoordinate,
        };
    }

    private async Task HandleAccessAddressSupplementaryTownNameChanged(
        AccessAddressSupplementaryTownNameChanged changedEvent,
        Guid eventId,
        long sequenceNumber,
        DateTime eventTimestamp)
    {
        var oldAccessAddress = _accessAddressIdToAccessAddress[changedEvent.Id];

        foreach (var unitAddressId in _accessAddressIdToUnitAddressIds[changedEvent.Id])
        {
            await _addressChangesChannel.Writer.WriteAsync(
                AccessAddressChangeConvert.SupplementaryTownNameChanged(
                    unitAddressId: unitAddressId,
                    eventId: eventId,
                    externalUpdated: changedEvent.ExternalUpdatedDate,
                    sequenceNumber: sequenceNumber,
                    eventTimestamp: eventTimestamp,
                    supplementaryTownNameBefore: oldAccessAddress.SupplementaryTownName,
                    supplementaryTownNameAfter: changedEvent.SupplementaryTownName))
                .ConfigureAwait(false);
        }

        _accessAddressIdToAccessAddress[changedEvent.Id] = oldAccessAddress with
        {
            SupplementaryTownName = changedEvent.SupplementaryTownName,
        };
    }

    private async Task HandleAccessAddressPlotIdChanged(
        AccessAddressPlotIdChanged changedEvent,
        Guid eventId,
        long sequenceNumber,
        DateTime eventTimestamp)
    {
        var oldAccessAddress = _accessAddressIdToAccessAddress[changedEvent.Id];

        foreach (var unitAddressId in _accessAddressIdToUnitAddressIds[changedEvent.Id])
        {
            await _addressChangesChannel.Writer.WriteAsync(
                AccessAddressChangeConvert.PlotIdChanged(
                    unitAddressId: unitAddressId,
                    eventId: eventId,
                    externalUpdated: changedEvent.ExternalUpdatedDate,
                    sequenceNumber: sequenceNumber,
                    eventTimestamp: eventTimestamp,
                    plotIdBefore: oldAccessAddress.PlotId,
                    plotIdAfter: changedEvent.PlotId))
                .ConfigureAwait(false);
        }

        _accessAddressIdToAccessAddress[changedEvent.Id] = oldAccessAddress with
        {
            PlotId = changedEvent.PlotId,
        };
    }

    private async Task HandleAccessAddressRoadIdChanged(
        AccessAddressRoadIdChanged changedEvent,
        Guid eventId,
        long sequenceNumber,
        DateTime eventTimestamp)
    {
        var oldAccessAddress = _accessAddressIdToAccessAddress[changedEvent.Id];

        foreach (var unitAddressId in _accessAddressIdToUnitAddressIds[changedEvent.Id])
        {
            await _addressChangesChannel.Writer.WriteAsync(
                AccessAddressChangeConvert.RoadIdChanged(
                    unitAddressId: unitAddressId,
                    eventId: eventId,
                    externalUpdated: changedEvent.ExternalUpdatedDate,
                    sequenceNumber: sequenceNumber,
                    eventTimestamp: eventTimestamp,
                    roadIdBefore: oldAccessAddress.RoadId,
                    roadIdAfter: changedEvent.RoadId))
                .ConfigureAwait(false);

            var roadNameBefore = _roadIdToRoadName[oldAccessAddress.RoadId];
            var roadNameAfter = _roadIdToRoadName[changedEvent.RoadId];
            await _addressChangesChannel.Writer.WriteAsync(
                AccessAddressChangeConvert.RoadIdChangedNewRoadName(
                    unitAddressId: unitAddressId,
                    eventId: eventId,
                    externalUpdated: changedEvent.ExternalUpdatedDate,
                    sequenceNumber: sequenceNumber,
                    eventTimestamp: eventTimestamp,
                    roadNameBefore: roadNameBefore,
                    roadNameAfter: roadNameAfter))
                .ConfigureAwait(false);
        }

        _accessAddressIdToAccessAddress[changedEvent.Id] = oldAccessAddress with
        {
            RoadId = changedEvent.RoadId,
        };

        _roadIdToAccessAddressIds[oldAccessAddress.RoadId].Remove(changedEvent.Id);
        _roadIdToAccessAddressIds[changedEvent.RoadId].Add(changedEvent.Id);
    }

    private async Task HandleAccessAddressDeleted(
        AccessAddressDeleted changedEvent,
        Guid eventId,
        long sequenceNumber,
        DateTime eventTimestamp)
    {
        foreach (var unitAddressId in _accessAddressIdToUnitAddressIds[changedEvent.Id])
        {
            await _addressChangesChannel.Writer.WriteAsync(
                AccessAddressChangeConvert.Deleted(
                    unitAddressId: unitAddressId,
                    eventId: eventId,
                    externalUpdated: changedEvent.ExternalUpdatedDate,
                    sequenceNumber: sequenceNumber,
                    eventTimestamp: eventTimestamp))
                .ConfigureAwait(false);
        }

        var accessAddress = _accessAddressIdToAccessAddress[changedEvent.Id];
        _roadIdToAccessAddressIds[accessAddress.RoadId].Remove(changedEvent.Id);

        _accessAddressIdToAccessAddress.Remove(changedEvent.Id);
        _accessAddressIdToUnitAddressIds.Remove(changedEvent.Id);
    }

    private async Task HandleUnitAddressCreated(
        UnitAddressCreated changedEvent,
        Guid eventId,
        long sequenceNumber,
        DateTime eventTimestamp)
    {

        if (_accessAddressIdToUnitAddressIds.ContainsKey(changedEvent.AccessAddressId))
        {
            _accessAddressIdToUnitAddressIds[changedEvent.AccessAddressId].Add(changedEvent.Id);

            await _addressChangesChannel.Writer.WriteAsync(
                UnitAddressChangeConvert.Created(
                    unitAddressId: changedEvent.Id,
                    eventId: eventId,
                    externalUpdated: changedEvent.ExternalCreatedDate,
                    sequenceNumber: sequenceNumber,
                    eventTimestamp: eventTimestamp))
                .ConfigureAwait(false);

            _unitAddressIdToUnitAddress.Add(
                changedEvent.Id,
                new UnitAddress(
                    accessAddressId: changedEvent.AccessAddressId,
                    status: changedEvent.Status,
                    floorName: changedEvent.FloorName,
                    suiteName: changedEvent.SuiteName,
                    externalUpdatedDate: changedEvent.ExternalUpdatedDate));
        }
        else
        {
            _logger.LogInformation("Could not load {AccessAddress} that the {UnitAddress} points to.", changedEvent.AccessAddressId, changedEvent.Id);
        }
    }

    private async Task HandleUnitAddressAccessAddressIdChanged(
        UnitAddressAccessAddressIdChanged changedEvent,
        Guid eventId,
        long sequenceNumber,
        DateTime eventTimestamp)
    {
        var oldUnitAddress = _unitAddressIdToUnitAddress[changedEvent.Id];

        await _addressChangesChannel.Writer.WriteAsync(
            UnitAddressChangeConvert.AccessAddressIdChanged(
                unitAddressId: changedEvent.Id,
                eventId: eventId,
                externalUpdated: changedEvent.ExternalUpdatedDate,
                sequenceNumber: sequenceNumber,
                eventTimestamp: eventTimestamp,
                accessAddressIdBefore: oldUnitAddress.AccessAddressId,
                accessAddressIdAfter: changedEvent.AccessAddressId))
            .ConfigureAwait(false);

        _unitAddressIdToUnitAddress[changedEvent.Id] = oldUnitAddress with
        {
            AccessAddressId = changedEvent.AccessAddressId,
            ExternalUpdatedDate = changedEvent.ExternalUpdatedDate
        };
    }

    private async Task HandleUnitAddressStatusChanged(
        UnitAddressStatusChanged changedEvent,
        Guid eventId,
        long sequenceNumber,
        DateTime eventTimestamp)
    {
        var oldUnitAddress = _unitAddressIdToUnitAddress[changedEvent.Id];

        await _addressChangesChannel.Writer.WriteAsync(
            UnitAddressChangeConvert.StatusChanged(
                unitAddressId: changedEvent.Id,
                eventId: eventId,
                externalUpdated: changedEvent.ExternalUpdatedDate,
                sequenceNumber: sequenceNumber,
                eventTimestamp: eventTimestamp,
                statusBefore: oldUnitAddress.Status,
                statusAfter: changedEvent.Status))
            .ConfigureAwait(false);

        _unitAddressIdToUnitAddress[changedEvent.Id] = oldUnitAddress with
        {
            Status = changedEvent.Status,
            ExternalUpdatedDate = changedEvent.ExternalUpdatedDate
        };
    }

    private async Task HandleUnitAddressFloorNameChanged(
        UnitAddressFloorNameChanged changedEvent,
        Guid eventId,
        long sequenceNumber,
        DateTime eventTimestamp)
    {
        var oldUnitAddress = _unitAddressIdToUnitAddress[changedEvent.Id];

        await _addressChangesChannel.Writer.WriteAsync(
            UnitAddressChangeConvert.FloorNameChanged(
                unitAddressId: changedEvent.Id,
                eventId: eventId,
                externalUpdated: changedEvent.ExternalUpdatedDate,
                sequenceNumber: sequenceNumber,
                eventTimestamp: eventTimestamp,
                floorNameBefore: oldUnitAddress.FloorName,
                floorNameAfter: changedEvent.FloorName))
            .ConfigureAwait(false);

        _unitAddressIdToUnitAddress[changedEvent.Id] = oldUnitAddress with
        {
            FloorName = changedEvent.FloorName,
            ExternalUpdatedDate = changedEvent.ExternalUpdatedDate
        };
    }

    private async Task HandleUnitAddressSuiteNameChanged(
        UnitAddressSuiteNameChanged changedEvent,
        Guid eventId,
        long sequenceNumber,
        DateTime eventTimestamp)
    {
        var oldUnitAddress = _unitAddressIdToUnitAddress[changedEvent.Id];

        await _addressChangesChannel.Writer.WriteAsync(
            UnitAddressChangeConvert.SuiteNameChanged(
                unitAddressId: changedEvent.Id,
                eventId: eventId,
                externalUpdated: changedEvent.ExternalUpdatedDate,
                sequenceNumber: sequenceNumber,
                eventTimestamp: eventTimestamp,
                suiteNameBefore: oldUnitAddress.SuiteName,
                suiteNameAfter: changedEvent.SuiteName))
            .ConfigureAwait(false);

        _unitAddressIdToUnitAddress[changedEvent.Id] = oldUnitAddress with
        {
            SuiteName = changedEvent.SuiteName,
            ExternalUpdatedDate = changedEvent.ExternalUpdatedDate
        };
    }

    private async Task HandleUnitAddressDeleted(
        UnitAddressDeleted changedEvent,
        Guid eventId,
        long sequenceNumber,
        DateTime eventTimestamp)
    {
        await _addressChangesChannel.Writer.WriteAsync(
            UnitAddressChangeConvert.Deleted(
                unitAddressId: changedEvent.Id,
                eventId: eventId,
                externalUpdated: changedEvent.ExternalUpdatedDate,
                eventTimestamp: eventTimestamp,
                sequenceNumber: sequenceNumber))
            .ConfigureAwait(false);

        var oldUnitAddress = _unitAddressIdToUnitAddress[changedEvent.Id];

        _unitAddressIdToUnitAddress.Remove(changedEvent.Id);

        // We have to check if the access address mapping still exists,
        // it can be deleted before the unit address.
        if (_accessAddressIdToUnitAddressIds.TryGetValue(oldUnitAddress.AccessAddressId, out var unitAddressIds))
        {
            unitAddressIds.Remove(changedEvent.Id);
        }
    }
}
