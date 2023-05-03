using OpenFTTH.Core.Address;

namespace OpenFTTH.AddressChangeIndexer;

internal static class AccessAddressChangeConvert
{
    public static AddressChange MunicipalCodeChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        string? municipalCodeBefore,
        string? municipalCodeAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressMunicipalCodeChanged,
            externalUpdated: externalUpdated,
            before: municipalCodeBefore,
            after: municipalCodeAfter);
    }

    public static AddressChange StatusChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        AccessAddressStatus statusBefore,
        AccessAddressStatus statusAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressStatusChanged,
            externalUpdated: externalUpdated,
            before: statusBefore.ToString(),
            after: statusAfter.ToString());
    }

    public static AddressChange RoadCodeChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        string roadCodeBefore,
        string roadCodeAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressRoadCodeChanged,
            externalUpdated: externalUpdated,
            before: roadCodeBefore,
            after: roadCodeAfter);
    }

    public static AddressChange HouseNumberChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        string houseNumberBefore,
        string houseNumberAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressHouseNumberChanged,
            externalUpdated: externalUpdated,
            before: houseNumberBefore,
            after: houseNumberAfter);
    }

    public static AddressChange SupplementaryTownNameChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        string? supplementaryTownNameBefore,
        string? supplementaryTownNameAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressSupplementaryTownNameChanged,
            externalUpdated: externalUpdated,
            before: supplementaryTownNameBefore,
            after: supplementaryTownNameAfter);
    }

    public static AddressChange PlotIdChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        string? plotIdBefore,
        string? plotIdAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressPlotIdChanged,
            externalUpdated: externalUpdated,
            before: plotIdBefore,
            after: plotIdAfter);
    }

    public static AddressChange RoadIdChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        Guid roadIdBefore,
        Guid roadIdAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressRoadIdChanged,
            externalUpdated: externalUpdated,
            before: roadIdBefore.ToString(),
            after: roadIdAfter.ToString());
    }

    public static AddressChange CoordinateChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        double eastCoordinateBefore,
        double northCoordinateBefore,
        double eastCoordinateAfter,
        double northCoordinateAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressCoordinateChanged,
            externalUpdated: externalUpdated,
            before: $"{eastCoordinateBefore},{northCoordinateBefore}",
            after: $"{eastCoordinateAfter},{northCoordinateAfter}");
    }
}
