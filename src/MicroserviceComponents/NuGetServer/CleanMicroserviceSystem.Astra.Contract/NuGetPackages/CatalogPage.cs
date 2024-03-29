using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Contract.NuGetPackages;


public class CatalogPage
{
    [JsonPropertyName("commitTimeStamp")]
    public DateTimeOffset CommitTimestamp { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("items")]
    public List<CatalogLeafItem> Items { get; set; }

    [JsonPropertyName("parent")]
    public string CatalogIndexUrl { get; set; }
}