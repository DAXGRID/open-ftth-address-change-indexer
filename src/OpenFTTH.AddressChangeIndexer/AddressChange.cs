namespace OpenFTTH.AddressChangeIndexer;

internal enum AddressChangeType
{
    AddressCreated,
    AccessAddressIdChanged,
    StatusChanged,
    FloorNameChanged,
    SuiteNameChanged,
    AccessAddressMunicipalCodeChanged,
    AccessAddressStatusChanged,
    AccessAddressRoadCodeChanged,
    AddressDeleted
}

internal sealed record AddressChange
{
    public Guid UnitAddressId { get; init; }
    public Guid EventId { get; init; }
    public AddressChangeType ChangeType { get; init; }
    public DateTime? ExternalUpdated { get; init; }
    public string? Before { get; init; }
    public string? After { get; init; }

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
