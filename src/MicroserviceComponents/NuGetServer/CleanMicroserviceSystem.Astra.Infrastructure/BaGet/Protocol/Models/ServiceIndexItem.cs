using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

public class ServiceIndexItem
{
    [JsonPropertyName("@id")]
    public string ResourceUrl { get; set; }

    [JsonPropertyName("@type")]
    public string Type { get; set; }

    [JsonPropertyName("comment")]
    public string Comment { get; set; }
}