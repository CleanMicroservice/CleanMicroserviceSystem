using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

public class SearchResultVersion
{
    [JsonPropertyName("@id")]
    public string RegistrationLeafUrl { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; }

    [JsonPropertyName("downloads")]
    public long Downloads { get; set; }
}