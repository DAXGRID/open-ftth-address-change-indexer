using OpenFTTH.Core.Address;

namespace OpenFTTH.AddressChangeIndexer;

internal static class UnitAddressChangeConvert
{
    public static AddressChange Created(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        long sequenceNumber)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.UnitAddressCreated,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            before: null,
            after: null);
    }

    public static AddressChange AccessAddressIdChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        long sequenceNumber,
        Guid accessAddressIdBefore,
        Guid accessAddressIdAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.UnitAddressAccessAddressIdChanged,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            before: accessAddressIdBefore.ToString(),
            after: accessAddressIdAfter.ToString());
    }

    public static AddressChange StatusChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        long sequenceNumber,
        UnitAddressStatus statusBefore,
        UnitAddressStatus statusAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.UnitAddressStatusChanged,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            before: Enum.GetName(typeof(UnitAddressStatus), statusBefore),
            after: Enum.GetName(typeof(UnitAddressStatus), statusAfter));
    }

    public static AddressChange FloorNameChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        long sequenceNumber,
        string? floorNameBefore,
        string? floorNameAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.UnitAddressFloorNameChanged,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            before: floorNameBefore,
            after: floorNameAfter);
    }

    public static AddressChange SuiteNameChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        long sequenceNumber,
        string? suiteNameBefore,
        string? suiteNameAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.UnitAddressSuiteNameChanged,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            before: suiteNameBefore,
            after: suiteNameAfter);
    }

    public static AddressChange Deleted(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        long sequenceNumber)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.UnitAddressDeleted,
            externalUpdated: externalUpdated,
            sequenceNumber: sequenceNumber,
            before: null,
            after: null);
    }
}
