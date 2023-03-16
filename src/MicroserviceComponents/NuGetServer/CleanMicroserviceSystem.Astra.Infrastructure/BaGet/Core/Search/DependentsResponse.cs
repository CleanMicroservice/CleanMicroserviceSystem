using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Search;

public class DependentsResponse
{
    [JsonPropertyName("totalHits")]
    public long TotalHits { get; set; }

    [JsonPropertyName("data")]
    public IReadOnlyList<PackageDependent> Data { get; set; }
}

public class PackageDependent
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("totalDownloads")]
    public long TotalDownloads { get; set; }
}