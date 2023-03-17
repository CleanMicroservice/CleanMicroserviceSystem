using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

public class SearchResultPackageType
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}