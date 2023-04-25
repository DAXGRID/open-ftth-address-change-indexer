namespace OpenFTTH.AddressChangeIndexer;

internal enum AddressChangeType
{
    New,
    CoordChanged,
    HouseNumber,
    StreetNumber,
    StatusChanged,
    Deleted
}

internal sealed record AddressChange
{
    public Guid Id { get; init; }
    public AddressChangeType ChangeType { get; init; }
    public double EastCoordinate { get; init;}
    public double NorthCoordinate { get; init;}

    public AddressChange(
        Guid id,
        AddressChangeType changeType,
        double eastCoordinate,
        double northCoordinate)
    {
        Id = id;
        ChangeType = changeType;
        EastCoordinate = eastCoordinate;
        NorthCoordinate = northCoordinate;
    }
}
