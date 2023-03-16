using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

public class PackageVersionsResponse
{
    [JsonPropertyName("versions")]
    public IReadOnlyList<string> Versions { get; set; }
}