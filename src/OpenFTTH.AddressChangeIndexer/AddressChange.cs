using OpenFTTH.Core.Address;

namespace OpenFTTH.AddressChangeIndexer;

internal enum AddressChangeType
{
    UnitAddressCreated,
    UnitAddressAccessAddressIdChanged,
    UnitAddressStatusChanged,
    UnitAddressFloorNameChanged,
    UnitAddressSuiteNameChanged,
    UnitAddressDeleted
}

internal sealed record AddressChange
{
    public Guid UnitAddressId { get; init; }
    public Guid EventId { get; init; }
    public AddressChangeType ChangeType { get; init; }
    public DateTime? ExternalUpdated { get; init; }
    public string? Before { get; init;}
    public string? After { get; init;}

    public AddressChange(
        Guid unitAddressId,
        Guid eventId,
        AddressChangeType changeType,
        DateTime? externalUpdated,
        string? before,
        string? after)
    {
        UnitAddressId = unitAddressId;
        EventId = eventId;
        ChangeType = changeType;
        ExternalUpdated = externalUpdated;
        Before = before;
        After = after;
    }
}

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
            changeType: AddressChangeType.UnitAddressCreated,
            externalUpdated: externalUpdated,
            before: null,
            after: null);
    }

    public static AddressChange AccessAddressIdChanged(
        Guid unitAddressId,
        Guid eventId,
        DateTime? externalUpdated,
        Guid before,
        Guid after)
    {
        return new AddressChange(
            unitAddressId: unitAddressId,
            eventId: eventId,
            changeType: AddressChangeType.UnitAddressAccessAddressIdChanged,
            externalUpdated: externalUpdated,
            before: before.ToString(),
            after: after.ToString());
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
            changeType: AddressChangeType.UnitAddressStatusChanged,
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
            changeType: AddressChangeType.UnitAddressFloorNameChanged,
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
            changeType: AddressChangeType.UnitAddressSuiteNameChanged,
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
            changeType: AddressChangeType.UnitAddressDeleted,
            externalUpdated: externalUpdated,
            before: null,
            after: null);
    }
}
