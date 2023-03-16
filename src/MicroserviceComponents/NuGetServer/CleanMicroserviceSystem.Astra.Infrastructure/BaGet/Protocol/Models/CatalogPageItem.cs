using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;


public class CatalogPageItem
{
    [JsonPropertyName("@id")]
    public string CatalogPageUrl { get; set; }

    [JsonPropertyName("commitTimeStamp")]
    public DateTimeOffset CommitTimestamp { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }
}