using OpenFTTH.Core.Address;
using OpenFTTH.Core.Address.Events;
using OpenFTTH.EventSourcing;
using System.Threading.Channels;

namespace OpenFTTH.AddressChangeIndexer;

internal sealed record AccessAddress
{
    public string MunicipalCode { get; init; }
    public AccessAddressStatus Status { get; init; }
    public string RoadCode { get; init; }
    public string HouseNumber { get; init; }
    public double EastCoordinate { get; init; }
    public double NorthCoordinate { get; init; }
    public string? TownName { get; init; }
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
        TownName = townName;
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
    public ChannelReader<AddressChange> AddressChanges => _addressChangesChannel.Reader;

    private readonly Channel<AddressChange> _addressChangesChannel = Channel.CreateUnbounded<AddressChange>();
    private readonly Dictionary<Guid, UnitAddress> _unitAddressIdToUnitAddress = new();
    private readonly Dictionary<Guid, AccessAddress> _accessAddressIdToAccessAddress = new();
    private readonly Dictionary<Guid, List<Guid>> _accessAddressIdToUnitAddressIds = new();

    public AddressChangeProjection()
    {
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
        switch (eventEnvelope.Data)
        {
            case (AccessAddressCreated accessAddressCreated):
                HandleAccessAddressCreated(accessAddressCreated);
                break;
            case (AccessAddressMunicipalCodeChanged accessAddressMunicipalCodeChanged):
                await HandleAccessAddressMunicipalCodeChanged(accessAddressMunicipalCodeChanged, eventEnvelope.EventId).ConfigureAwait(false);
                break;
            case (AccessAddressStatusChanged accessAddressStatusChanged):
                HandleAccessAddressStatusChanged(accessAddressStatusChanged);
                break;
            case (AccessAddressRoadCodeChanged accessAddressRoadCodeChanged):
                HandleAccessAddressRoadCodeChanged(accessAddressRoadCodeChanged);
                break;
            case (AccessAddressHouseNumberChanged accessAddressHouseNumberChanged):
                HandleAccessAddressHouseNumberChanged(accessAddressHouseNumberChanged);
                break;
            case (AccessAddressCoordinateChanged accessAddressCoordinateChanged):
                HandleAccessAddressCoordinateChanged(accessAddressCoordinateChanged);
                break;
            case (AccessAddressSupplementaryTownNameChanged accessAddressSupplementaryTownNameChanged):
                HandleAccessAddressSupplementaryTownNameChanged(accessAddressSupplementaryTownNameChanged);
                break;
            case (AccessAddressPlotIdChanged handleAccessAddressPlotIdChanged):
                HandleAccessAddressPlotIdChanged(handleAccessAddressPlotIdChanged);
                break;
            case (AccessAddressRoadIdChanged handleAccessAddressRoadIdChanged):
                HandleAccessAddressRoadIdChanged(handleAccessAddressRoadIdChanged);
                break;
            case (AccessAddressDeleted accessAddressDeleted):
                HandleAccessAddressDeleted(accessAddressDeleted);
                break;

            case UnitAddressCreated unitAddressCreated:
                await HandleUnitAddressCreated(unitAddressCreated, eventEnvelope.EventId).ConfigureAwait(false);
                break;
            case UnitAddressAccessAddressIdChanged unitAddressAccessAddressIdChanged:
                await HandleUnitAddressAccessAddressIdChanged(unitAddressAccessAddressIdChanged, eventEnvelope.EventId).ConfigureAwait(false);
                break;
            case UnitAddressStatusChanged unitAddressStatusChanged:
                await HandleUnitAddressStatusChanged(unitAddressStatusChanged, eventEnvelope.EventId).ConfigureAwait(false);
                break;
            case UnitAddressFloorNameChanged unitAddressFloorNameChanged:
                await HandleUnitAddressFloorNameChanged(unitAddressFloorNameChanged, eventEnvelope.EventId).ConfigureAwait(false);
                break;
            case UnitAddressSuiteNameChanged unitAddressSuiteNameChanged:
                await HandleUnitAddressSuiteNameChanged(unitAddressSuiteNameChanged, eventEnvelope.EventId).ConfigureAwait(false);
                break;
            case UnitAddressDeleted unitAddressDeleted:
                await HandleUnitAddressDeleted(unitAddressDeleted, eventEnvelope.EventId).ConfigureAwait(false);
                break;

            default:
                throw new ArgumentException(
                    $"Could not handle typeof '{eventEnvelope.Data.GetType().Name}'");
        }
    }

    private void HandleAccessAddressCreated(AccessAddressCreated accessAddressCreated)
    {
        _accessAddressIdToUnitAddressIds.Add(accessAddressCreated.Id, new());

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

    private async Task HandleAccessAddressMunicipalCodeChanged(AccessAddressMunicipalCodeChanged changedEvent, Guid eventId)
    {
        var oldAccessAddress = _accessAddressIdToAccessAddress[changedEvent.Id];

        foreach (var unitAddressId in _accessAddressIdToUnitAddressIds[changedEvent.Id])
        {
            await _addressChangesChannel.Writer.WriteAsync(
                UnitAddressChangeConvert.SuiteNameChanged(
                    unitAddressId: unitAddressId,
                    eventId: eventId,
                    externalUpdated: changedEvent.ExternalUpdatedDate,
                    suiteNameBefore: oldAccessAddress.MunicipalCode,
                    suiteNameAfter: changedEvent.MunicipalCode))
                .ConfigureAwait(false);
        }

        _accessAddressIdToAccessAddress[changedEvent.Id] = oldAccessAddress with
        {
            MunicipalCode = changedEvent.MunicipalCode,
        };
    }

    private void HandleAccessAddressStatusChanged(AccessAddressStatusChanged changedEvent)
    {
        var oldAccessAddress = _accessAddressIdToAccessAddress[changedEvent.Id];
        _accessAddressIdToAccessAddress[changedEvent.Id] = oldAccessAddress with
        {
            Status = changedEvent.Status
        };
    }

    private void HandleAccessAddressRoadCodeChanged(AccessAddressRoadCodeChanged changedEvent)
    {
        var oldAccessAddress = _accessAddressIdToAccessAddress[changedEvent.Id];
        _accessAddressIdToAccessAddress[changedEvent.Id] = oldAccessAddress with
        {
            RoadCode = changedEvent.RoadCode,
        };
    }

    private void HandleAccessAddressHouseNumberChanged(AccessAddressHouseNumberChanged changedEvent)
    {
        var oldAccessAddress = _accessAddressIdToAccessAddress[changedEvent.Id];
        _accessAddressIdToAccessAddress[changedEvent.Id] = oldAccessAddress with
        {
            HouseNumber = changedEvent.HouseNumber,
        };
    }

    private void HandleAccessAddressCoordinateChanged(AccessAddressCoordinateChanged changedEvent)
    {
        var oldAccessAddress = _accessAddressIdToAccessAddress[changedEvent.Id];
        _accessAddressIdToAccessAddress[changedEvent.Id] = oldAccessAddress with
        {
            EastCoordinate = changedEvent.EastCoordinate,
            NorthCoordinate = changedEvent.NorthCoordinate,
        };
    }

    private void HandleAccessAddressSupplementaryTownNameChanged(AccessAddressSupplementaryTownNameChanged changedEvent)
    {
        var oldAccessAddress = _accessAddressIdToAccessAddress[changedEvent.Id];
        _accessAddressIdToAccessAddress[changedEvent.Id] = oldAccessAddress with
        {
            TownName = changedEvent.SupplementaryTownName,
        };
    }

    private void HandleAccessAddressPlotIdChanged(AccessAddressPlotIdChanged changedEvent)
    {
        var oldAccessAddress = _accessAddressIdToAccessAddress[changedEvent.Id];
        _accessAddressIdToAccessAddress[changedEvent.Id] = oldAccessAddress with
        {
            PlotId = changedEvent.PlotId,
        };
    }

    private void HandleAccessAddressRoadIdChanged(AccessAddressRoadIdChanged changedEvent)
    {
        var oldAccessAddress = _accessAddressIdToAccessAddress[changedEvent.Id];
        _accessAddressIdToAccessAddress[changedEvent.Id] = oldAccessAddress with
        {
            RoadId = changedEvent.RoadId,
        };
    }

    private void HandleAccessAddressDeleted(AccessAddressDeleted accessAddressDeleted)
    {
        _accessAddressIdToAccessAddress.Remove(accessAddressDeleted.Id);
    }

    private async Task HandleUnitAddressCreated(UnitAddressCreated changedEvent, Guid eventId)
    {
        _accessAddressIdToUnitAddressIds[changedEvent.AccessAddressId].Add(changedEvent.Id);

        await _addressChangesChannel.Writer.WriteAsync(
            UnitAddressChangeConvert.Created(
                unitAddressId: changedEvent.Id,
                eventId: eventId,
                externalUpdated: changedEvent.ExternalCreatedDate))
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

    private async Task HandleUnitAddressAccessAddressIdChanged(UnitAddressAccessAddressIdChanged changedEvent, Guid eventId)
    {
        var oldUnitAddress = _unitAddressIdToUnitAddress[changedEvent.Id];

        await _addressChangesChannel.Writer.WriteAsync(
            UnitAddressChangeConvert.AccessAddressIdChanged(
                unitAddressId: changedEvent.Id,
                eventId: eventId,
                externalUpdated: changedEvent.ExternalUpdatedDate,
                accessAddressIdBefore: oldUnitAddress.AccessAddressId,
                accessAddressIdAfter: changedEvent.AccessAddressId))
            .ConfigureAwait(false);

        _unitAddressIdToUnitAddress[changedEvent.Id] = oldUnitAddress with
        {
            AccessAddressId = changedEvent.AccessAddressId,
            ExternalUpdatedDate = changedEvent.ExternalUpdatedDate
        };
    }

    private async Task HandleUnitAddressStatusChanged(UnitAddressStatusChanged changedEvent, Guid eventId)
    {
        await _addressChangesChannel.Writer.WriteAsync(
            UnitAddressChangeConvert.StatusChanged(
                unitAddressId: changedEvent.Id,
                eventId: eventId,
                externalUpdated: changedEvent.ExternalUpdatedDate,
                statusBefore: changedEvent.Status,
                statusAfter: _unitAddressIdToUnitAddress[changedEvent.Id].Status))
            .ConfigureAwait(false);

        var oldUnitAddress = _unitAddressIdToUnitAddress[changedEvent.Id];
        _unitAddressIdToUnitAddress[changedEvent.Id] = oldUnitAddress with
        {
            Status = changedEvent.Status,
            ExternalUpdatedDate = changedEvent.ExternalUpdatedDate
        };
    }

    private async Task HandleUnitAddressFloorNameChanged(UnitAddressFloorNameChanged changedEvent, Guid eventId)
    {
        var oldUnitAddress = _unitAddressIdToUnitAddress[changedEvent.Id];

        await _addressChangesChannel.Writer.WriteAsync(
            UnitAddressChangeConvert.FloorNameChanged(
                unitAddressId: changedEvent.Id,
                eventId: eventId,
                externalUpdated: changedEvent.ExternalUpdatedDate,
                floorNameBefore: oldUnitAddress.FloorName,
                floorNameAfter: changedEvent.FloorName))
            .ConfigureAwait(false);

        _unitAddressIdToUnitAddress[changedEvent.Id] = oldUnitAddress with
        {
            FloorName = changedEvent.FloorName,
            ExternalUpdatedDate = changedEvent.ExternalUpdatedDate
        };
    }

    private async Task HandleUnitAddressSuiteNameChanged(UnitAddressSuiteNameChanged changedEvent, Guid eventId)
    {
        var oldUnitAddress = _unitAddressIdToUnitAddress[changedEvent.Id];

        await _addressChangesChannel.Writer.WriteAsync(
            UnitAddressChangeConvert.SuiteNameChanged(
                unitAddressId: changedEvent.Id,
                eventId: eventId,
                externalUpdated: changedEvent.ExternalUpdatedDate,
                suiteNameBefore: oldUnitAddress.SuiteName,
                suiteNameAfter: changedEvent.SuiteName))
            .ConfigureAwait(false);

        _unitAddressIdToUnitAddress[changedEvent.Id] = oldUnitAddress with
        {
            SuiteName = changedEvent.SuiteName,
            ExternalUpdatedDate = changedEvent.ExternalUpdatedDate
        };
    }

    private async Task HandleUnitAddressDeleted(UnitAddressDeleted changedEvent, Guid eventId)
    {
        await _addressChangesChannel.Writer.WriteAsync(
            UnitAddressChangeConvert.Deleted(
                unitAddressId: changedEvent.Id,
                eventId: eventId,
                externalUpdated: changedEvent.ExternalUpdatedDate))
            .ConfigureAwait(false);

        _unitAddressIdToUnitAddress.Remove(changedEvent.Id);
    }
}
