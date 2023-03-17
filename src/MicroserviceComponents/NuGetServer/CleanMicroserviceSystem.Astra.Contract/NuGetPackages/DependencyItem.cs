using System.Text.Json.Serialization;
using CleanMicroserviceSystem.Astra.Contract.NuGetPackages.Converters;

namespace CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

public class DependencyItem
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("range")]
    [JsonConverter(typeof(PackageDependencyRangeJsonConverter))]
    public string Range { get; set; }
}