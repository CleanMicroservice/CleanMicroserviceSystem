using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Contract.NuGetPackages;


public class CatalogLeafItem : ICatalogLeafItem
{
    [JsonPropertyName("@id")]
    public string CatalogLeafUrl { get; set; }

    [JsonPropertyName("@type")]
    public string Type { get; set; }

    [JsonPropertyName("commitTimeStamp")]
    public DateTimeOffset CommitTimestamp { get; set; }

    [JsonPropertyName("nuget:id")]
    public string PackageId { get; set; }

    [JsonPropertyName("nuget:version")]
    public string PackageVersion { get; set; }
}