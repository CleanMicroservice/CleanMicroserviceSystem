using System.Text.Json.Serialization;
using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Metadata;

public class BaGetPackageMetadata : PackageMetadata
{
    [JsonPropertyName("downloads")]
    public long Downloads { get; set; }

    [JsonPropertyName("hasReadme")]
    public bool HasReadme { get; set; }

    [JsonPropertyName("packageTypes")]
    public IReadOnlyList<string> PackageTypes { get; set; }

    [JsonPropertyName("releaseNotes")]
    public string ReleaseNotes { get; set; }

    [JsonPropertyName("repositoryUrl")]
    public string RepositoryUrl { get; set; }

    [JsonPropertyName("repositoryType")]
    public string RepositoryType { get; set; }
}