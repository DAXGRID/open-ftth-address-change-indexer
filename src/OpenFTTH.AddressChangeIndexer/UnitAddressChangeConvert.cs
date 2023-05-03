using OpenFTTH.Core.Address;

namespace OpenFTTH.AddressChangeIndexer;

internal static class UnitAddressChangeConvert
{
    public static AddressChange Created(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AddressCreated,
            externalUpdated: externalUpdated,
            before: null,
            after: null);
    }

    public static AddressChange AccessAddressIdChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        Guid accessAddressIdBefore,
        Guid accessAddressIdAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AccessAddressIdChanged,
            externalUpdated: externalUpdated,
            before: accessAddressIdBefore.ToString(),
            after: accessAddressIdAfter.ToString());
    }

    public static AddressChange StatusChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        UnitAddressStatus statusBefore,
        UnitAddressStatus statusAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.StatusChanged,
            externalUpdated: externalUpdated,
            before: Enum.GetName(typeof(UnitAddressStatus), statusBefore),
            after: Enum.GetName(typeof(UnitAddressStatus), statusAfter));
    }

    public static AddressChange FloorNameChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        string? floorNameBefore,
        string? floorNameAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.FloorNameChanged,
            externalUpdated: externalUpdated,
            before: floorNameBefore,
            after: floorNameAfter);
    }

    public static AddressChange SuiteNameChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        string? suiteNameBefore,
        string? suiteNameAfter)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.SuiteNameChanged,
            externalUpdated: externalUpdated,
            before: suiteNameBefore,
            after: suiteNameAfter);
    }

    public static AddressChange Deleted(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.AddressDeleted,
            externalUpdated: externalUpdated,
            before: null,
            after: null);
    }
}
