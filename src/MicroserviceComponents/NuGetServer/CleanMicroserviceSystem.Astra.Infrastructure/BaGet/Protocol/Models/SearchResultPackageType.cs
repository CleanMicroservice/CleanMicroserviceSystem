using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

public class SearchResultPackageType
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}