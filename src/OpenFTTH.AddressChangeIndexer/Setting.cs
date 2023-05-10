using System.Text.Json.Serialization;

namespace OpenFTTH.AddressChangeIndexer;

internal sealed record Setting
{
    [JsonPropertyName("eventStoreConnectionString")]
    public string EventStoreConnectionString { get; init; }

    [JsonPropertyName("geoDatabaseConnectionString")]
    public string GeoDatabaseConnectionString { get; init; }

    [JsonConstructor]
    public Setting(
        string eventStoreConnectionString,
        string geoDatabaseConnectionString)
    {
        EventStoreConnectionString = eventStoreConnectionString;
        GeoDatabaseConnectionString = geoDatabaseConnectionString;
    }
}
