using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

public class PackageMetadata
{
    [JsonPropertyName("@id")]
    public string CatalogLeafUrl { get; set; }

    [JsonPropertyName("id")]
    public string PackageId { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; }

    [JsonPropertyName("authors")]
    public string Authors { get; set; }

    [JsonPropertyName("dependencyGroups")]
    public IReadOnlyList<DependencyGroupItem> DependencyGroups { get; set; }

    [JsonPropertyName("deprecation")]
    public PackageDeprecation Deprecation { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("iconUrl")]
    public string IconUrl { get; set; }

    [JsonPropertyName("language")]
    public string Language { get; set; }

    [JsonPropertyName("licenseUrl")]
    public string LicenseUrl { get; set; }

    [JsonPropertyName("listed")]
    public bool? Listed { get; set; }

    [JsonPropertyName("minClientVersion")]
    public string MinClientVersion { get; set; }

    [JsonPropertyName("packageContent")]
    public string PackageContentUrl { get; set; }

    [JsonPropertyName("projectUrl")]
    public string ProjectUrl { get; set; }

    [JsonPropertyName("published")]
    public DateTimeOffset Published { get; set; }

    [JsonPropertyName("requireLicenseAcceptance")]
    public bool RequireLicenseAcceptance { get; set; }

    [JsonPropertyName("summary")]
    public string Summary { get; set; }

    [JsonPropertyName("tags")]
    public IReadOnlyList<string> Tags { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }
}