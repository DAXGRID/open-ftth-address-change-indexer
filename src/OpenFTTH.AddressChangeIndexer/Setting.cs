using System.Text.Json.Serialization;

namespace OpenFTTH.AddressChangeIndexer;

internal sealed record Setting
{
    [JsonPropertyName("eventStoreConnectionString")]
    public string EventStoreConnectionString { get; init; }

    [JsonConstructor]
    public Setting(string eventStoreConnectionString)
    {
        EventStoreConnectionString = eventStoreConnectionString;
    }
}
