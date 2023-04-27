namespace OpenFTTH.AddressChangeIndexer;

internal enum AddressChangeType
{
    New,
    CoordinateChanged,
    Deleted
}

internal sealed record AddressChange
{
    public Guid Id { get; init; }
    public Guid EventId { get; init; }
    public AddressChangeType ChangeType { get; init; }
    public string Before { get; init;}
    public string After { get; init;}

    public AddressChange(
        Guid id,
        Guid eventId,
        AddressChangeType changeType,
        string before,
        string after)
    {
        Id = id;
        EventId = eventId;
        ChangeType = changeType;
        Before = before;
        After = after;
    }
}
