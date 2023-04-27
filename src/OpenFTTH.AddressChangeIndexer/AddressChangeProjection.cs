using OpenFTTH.Core.Address;
using OpenFTTH.Core.Address.Events;
using OpenFTTH.EventSourcing;

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
    private Dictionary<Guid, UnitAddress> _unitAddressIdToUnitAddress = new();
    public readonly Dictionary<Guid, AccessAddress> _accessAddressIdToAccessAddress = new();

    public AddressChangeProjection()
    {
        ProjectEventAsync<UnitAddressCreated>(ProjectAsync);
        ProjectEventAsync<UnitAddressAccessAddressIdChanged>(ProjectAsync);
        ProjectEventAsync<UnitAddressStatusChanged>(ProjectAsync);
        ProjectEventAsync<UnitAddressFloorNameChanged>(ProjectAsync);
        ProjectEventAsync<UnitAddressSuiteNameChanged>(ProjectAsync);
        ProjectEventAsync<UnitAddressDeleted>(ProjectAsync);

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
    }

    private Task ProjectAsync(IEventEnvelope eventEnvelope)
    {
        switch (eventEnvelope.Data)
        {
            case (AccessAddressCreated accessAddressCreated):
                HandleAccessAddressCreated(accessAddressCreated);
                break;
            case (AccessAddressMunicipalCodeChanged accessAddressMunicipalCodeChanged):
                HandleAccessAddressMunicipalCodeChanged(accessAddressMunicipalCodeChanged);
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
                HandleUnitAddressCreated(unitAddressCreated);
                break;
            case UnitAddressAccessAddressIdChanged unitAddressAccessAddressIdChanged:
                HandleUnitAddressAccessAddressIdChanged(unitAddressAccessAddressIdChanged);
                break;
            case UnitAddressStatusChanged unitAddressStatusChanged:
                HandleUnitAddressStatusChanged(unitAddressStatusChanged);
                break;
            case UnitAddressFloorNameChanged unitAddressFloorNameChanged:
                HandleUnitAddressFloorNameChanged(unitAddressFloorNameChanged);
                break;
            case UnitAddressSuiteNameChanged unitAddressSuiteNameChanged:
                HandleUnitAddressSuiteNameChanged(unitAddressSuiteNameChanged);
                break;
            case UnitAddressDeleted unitAddressDeleted:
                HandleUnitAddressDeleted(unitAddressDeleted);
                break;

            default:
                throw new ArgumentException(
                    $"Could not handle typeof '{eventEnvelope.Data.GetType().Name}'");
        }

        return Task.CompletedTask;
    }

    private void HandleAccessAddressCreated(AccessAddressCreated accessAddressCreated)
    {
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

    private void HandleAccessAddressMunicipalCodeChanged(AccessAddressMunicipalCodeChanged changedEvent)
    {
        var oldAccessAddress = _accessAddressIdToAccessAddress[changedEvent.Id];
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

    private void HandleUnitAddressCreated(UnitAddressCreated unitAddressCreated)
    {
        _unitAddressIdToUnitAddress.Add(
            unitAddressCreated.Id,
            new UnitAddress(
                accessAddressId: unitAddressCreated.AccessAddressId,
                status: unitAddressCreated.Status,
                floorName: unitAddressCreated.FloorName,
                suiteName: unitAddressCreated.SuiteName,
                externalUpdatedDate: unitAddressCreated.ExternalUpdatedDate));
    }

    private void HandleUnitAddressAccessAddressIdChanged(UnitAddressAccessAddressIdChanged changedEvent)
    {
        var oldUnitAddress = _unitAddressIdToUnitAddress[changedEvent.Id];
        _unitAddressIdToUnitAddress[changedEvent.Id] = oldUnitAddress with
        {
            AccessAddressId = changedEvent.AccessAddressId,
            ExternalUpdatedDate = changedEvent.ExternalUpdatedDate
        };
    }

    private void HandleUnitAddressStatusChanged(UnitAddressStatusChanged changedEvent)
    {
        var oldUnitAddress = _unitAddressIdToUnitAddress[changedEvent.Id];
        _unitAddressIdToUnitAddress[changedEvent.Id] = oldUnitAddress with
        {
            Status = changedEvent.Status,
            ExternalUpdatedDate = changedEvent.ExternalUpdatedDate
        };
    }

    private void HandleUnitAddressFloorNameChanged(UnitAddressFloorNameChanged changedEvent)
    {
        var oldUnitAddress = _unitAddressIdToUnitAddress[changedEvent.Id];
        _unitAddressIdToUnitAddress[changedEvent.Id] = oldUnitAddress with
        {
            FloorName = changedEvent.FloorName,
            ExternalUpdatedDate = changedEvent.ExternalUpdatedDate
        };
    }

    private void HandleUnitAddressSuiteNameChanged(UnitAddressSuiteNameChanged changedEvent)
    {
        var oldUnitAddress = _unitAddressIdToUnitAddress[changedEvent.Id];
        _unitAddressIdToUnitAddress[changedEvent.Id] = oldUnitAddress with
        {
            SuiteName = changedEvent.SuiteName,
            ExternalUpdatedDate = changedEvent.ExternalUpdatedDate
        };
    }

    private void HandleUnitAddressDeleted(UnitAddressDeleted changedEvent)
    {
        _unitAddressIdToUnitAddress.Remove(changedEvent.Id);
    }
}
