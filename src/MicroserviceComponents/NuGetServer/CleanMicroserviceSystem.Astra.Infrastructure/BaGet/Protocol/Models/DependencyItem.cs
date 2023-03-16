using System.Text.Json.Serialization;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Converters;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

public class DependencyItem
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("range")]
    [JsonConverter(typeof(PackageDependencyRangeJsonConverter))]
    public string Range { get; set; }
}