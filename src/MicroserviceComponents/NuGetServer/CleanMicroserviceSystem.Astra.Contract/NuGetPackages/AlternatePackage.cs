using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

public class AlternatePackage
{
    [JsonPropertyName("@id")]
    public string Url { get; set; }

    [JsonPropertyName("@type")]
    public string Type { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("range")]
    public string Range { get; set; }
}