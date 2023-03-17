using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Contract.NuGetPackages;


public class CatalogLeaf : ICatalogLeafItem
{
    [JsonPropertyName("@id")]
    public string CatalogLeafUrl { get; set; }

    [JsonPropertyName("@type")]
    public IReadOnlyList<string> Type { get; set; }

    [JsonPropertyName("catalog:commitId")]
    public string CommitId { get; set; }

    [JsonPropertyName("catalog:commitTimeStamp")]
    public DateTimeOffset CommitTimestamp { get; set; }

    [JsonPropertyName("id")]
    public string PackageId { get; set; }

    [JsonPropertyName("published")]
    public DateTimeOffset Published { get; set; }

    [JsonPropertyName("version")]
    public string PackageVersion { get; set; }
}