using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

public class SearchResponse
{
    public static SearchResponse Empty = new SearchResponse();

    [JsonPropertyName("@context")]
    public SearchContext Context { get; set; }

    [JsonPropertyName("totalHits")]
    public long TotalHits { get; set; }

    [JsonPropertyName("data")]
    public IReadOnlyList<SearchResult> Data { get; set; }
}