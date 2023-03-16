using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

public class PackageDeprecation
{
    [JsonPropertyName("@id")]
    public string CatalogLeafUrl { get; set; }

    [JsonPropertyName("reasons")]
    public IReadOnlyList<string> Reasons { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("alternatePackage")]
    public AlternatePackage AlternatePackage { get; set; }
}