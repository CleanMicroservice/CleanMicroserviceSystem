using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

public class AutocompleteResponse
{
    [JsonPropertyName("@context")]
    public AutocompleteContext Context { get; set; }

    [JsonPropertyName("totalHits")]
    public long TotalHits { get; set; }

    [JsonPropertyName("data")]
    public IReadOnlyList<string> Data { get; set; }
}