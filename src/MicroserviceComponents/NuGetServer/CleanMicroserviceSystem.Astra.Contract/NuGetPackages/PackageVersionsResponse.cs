using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

public class PackageVersionsResponse
{
    [JsonPropertyName("versions")]
    public IReadOnlyList<string> Versions { get; set; }
}