using OpenFTTH.Core.Address;

namespace OpenFTTH.AddressChangeIndexer;

internal static class AccessAddressChangeConvert
{
    public static AddressChange MunicipalCodeChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        long sequenceNumber,
        DateTime eventTimestamp,
        string? municipalCodeBefore,
        string? municipalCodeAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressMunicipalCodeChanged,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
            before: municipalCodeBefore,
            after: municipalCodeAfter);
    }

    public static AddressChange StatusChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        long sequenceNumber,
        DateTime eventTimestamp,
        AccessAddressStatus statusBefore,
        AccessAddressStatus statusAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressStatusChanged,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
            before: statusBefore.ToString(),
            after: statusAfter.ToString());
    }

    public static AddressChange RoadCodeChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        long sequenceNumber,
        DateTime eventTimestamp,
        string roadCodeBefore,
        string roadCodeAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressRoadCodeChanged,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
            before: roadCodeBefore,
            after: roadCodeAfter);
    }

    public static AddressChange HouseNumberChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        long sequenceNumber,
        DateTime eventTimestamp,
        string houseNumberBefore,
        string houseNumberAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressHouseNumberChanged,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
            before: houseNumberBefore,
            after: houseNumberAfter);
    }

    public static AddressChange SupplementaryTownNameChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        long sequenceNumber,
        DateTime eventTimestamp,
        string? supplementaryTownNameBefore,
        string? supplementaryTownNameAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressSupplementaryTownNameChanged,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
            before: supplementaryTownNameBefore,
            after: supplementaryTownNameAfter);
    }

    public static AddressChange PlotIdChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        long sequenceNumber,
        DateTime eventTimestamp,
        string? plotIdBefore,
        string? plotIdAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressPlotIdChanged,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
            before: plotIdBefore,
            after: plotIdAfter);
    }

    public static AddressChange RoadIdChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        long sequenceNumber,
        DateTime eventTimestamp,
        Guid roadIdBefore,
        Guid roadIdAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressRoadIdChanged,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
            before: roadIdBefore.ToString(),
            after: roadIdAfter.ToString());
    }

    public static AddressChange CoordinateChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        long sequenceNumber,
        DateTime eventTimestamp,
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
            sequenceNumber: sequenceNumber,
            eventTimestamp: eventTimestamp,
            before: $"{eastCoordinateBefore},{northCoordinateBefore}",
            after: $"{eastCoordinateAfter},{northCoordinateAfter}");
    }
}
