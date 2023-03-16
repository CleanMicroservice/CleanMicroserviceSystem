using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

public class ServiceIndexResponse
{
    [JsonPropertyName("version")]
    public string Version { get; set; }

    [JsonPropertyName("resources")]
    public IReadOnlyList<ServiceIndexItem> Resources { get; set; }
}