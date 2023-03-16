using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

public class SearchResponse
{
    [JsonPropertyName("@context")]
    public SearchContext Context { get; set; }

    [JsonPropertyName("totalHits")]
    public long TotalHits { get; set; }

    [JsonPropertyName("data")]
    public IReadOnlyList<SearchResult> Data { get; set; }
}