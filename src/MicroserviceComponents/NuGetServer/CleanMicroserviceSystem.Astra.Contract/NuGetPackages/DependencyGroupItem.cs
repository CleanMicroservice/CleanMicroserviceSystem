using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

public class DependencyGroupItem
{
    [JsonPropertyName("targetFramework")]
    public string TargetFramework { get; set; }

    [JsonPropertyName("dependencies")]
    public List<DependencyItem> Dependencies { get; set; }
}