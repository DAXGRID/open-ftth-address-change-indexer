namespace OpenFTTH.AddressChangeIndexer;

internal enum AddressChangeType
{
    UnitAddressCreated,
    UnitAddressAccessAddressIdChanged,
    UnitAddressStatusChanged,
    UnitAddressFloorNameChanged,
    UnitAddressSuiteNameChanged,
    AccessAddressMunicipalCodeChanged,
    AccessAddressStatusChanged,
    AccessAddressRoadCodeChanged,
    AccessAddressHouseNumberChanged,
    AccessAddressSupplementaryTownNameChanged,
    AccessAddressPlotIdChanged,
    AccessAddressRoadIdChanged,
    AccessAddressRoadIdChangedNewRoadName,
    AccessAddressCoordinateChanged,
    AccessAddressDeleted,
    UnitAddressDeleted
}

internal sealed record AddressChange
{
    public Guid UnitAddressId { get; init; }
    public Guid EventId { get; init; }
    public AddressChangeType ChangeType { get; init; }
    public DateTime EventTimestamp { get; init; }
    public DateTime? ExternalUpdated { get; init; }
    public long SequenceNumber { get; init; }
    public string? Before { get; init; }
    public string? After { get; init; }

    public AddressChange(
        Guid unitAddressId,
        Guid eventId,
        AddressChangeType changeType,
        DateTime? externalUpdated,
        long sequenceNumber,
        DateTime eventTimestamp,
        string? before = null,
        string? after = null)
    {
        UnitAddressId = unitAddressId;
        EventId = eventId;
        ChangeType = changeType;
        ExternalUpdated = externalUpdated;
        SequenceNumber = sequenceNumber;
        EventTimestamp = eventTimestamp;
        Before = before;
        After = after;
    }
}
