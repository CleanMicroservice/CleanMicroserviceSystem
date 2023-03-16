using System.Text.Json.Serialization;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Converters;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

public class SearchResult
{
    [JsonPropertyName("id")]
    public string PackageId { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("authors")]
    [JsonConverter(typeof(StringOrStringArrayJsonConverter))]
    public IReadOnlyList<string> Authors { get; set; }

    [JsonPropertyName("iconUrl")]
    public string IconUrl { get; set; }

    [JsonPropertyName("licenseUrl")]
    public string LicenseUrl { get; set; }

    [JsonPropertyName("packageTypes")]
    public IReadOnlyList<SearchResultPackageType> PackageTypes { get; set; }

    [JsonPropertyName("projectUrl")]
    public string ProjectUrl { get; set; }

    [JsonPropertyName("registration")]
    public string RegistrationIndexUrl { get; set; }

    [JsonPropertyName("summary")]
    public string Summary { get; set; }

    [JsonPropertyName("tags")]
    public IReadOnlyList<string> Tags { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("totalDownloads")]
    public long TotalDownloads { get; set; }

    [JsonPropertyName("versions")]
    public IReadOnlyList<SearchResultVersion> Versions { get; set; }
}