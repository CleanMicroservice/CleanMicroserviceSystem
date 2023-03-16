using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;


public class CatalogIndex
{
    [JsonPropertyName("commitTimeStamp")]
    public DateTimeOffset CommitTimestamp { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("items")]
    public List<CatalogPageItem> Items { get; set; }
}