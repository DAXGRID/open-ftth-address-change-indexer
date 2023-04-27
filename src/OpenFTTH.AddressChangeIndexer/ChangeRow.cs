namespace OpenFTTH.AddressChangeIndexer;

internal sealed record AddressChangeRow
{
    public Guid Id { get; init; }
    public string ChangeType { get; init; }
    public string? Before { get; init; }
    public string? After { get; init; }
    public DateTime? ExternalUpdated { get; init; }

    public AddressChangeRow(
        Guid id,
        string changeType,
        string before,
        string after,
        DateTime? externalUpdated)
    {
        Id = id;
        ChangeType = changeType;
        Before = before;
        After = after;
        ExternalUpdated = externalUpdated;
    }
}
