using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

public class RegistrationIndexPage
{
    [JsonPropertyName("@id")]
    public string RegistrationPageUrl { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("items")]
    public IReadOnlyList<RegistrationIndexPageItem> ItemsOrNull { get; set; }

    [JsonPropertyName("lower")]
    public string Lower { get; set; }

    [JsonPropertyName("upper")]
    public string Upper { get; set; }
}