using System.Text.Json.Serialization;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Metadata;

public class BaGetRegistrationIndexResponse
{
    #region Original properties from RegistrationIndexResponse.

    [JsonPropertyName("@id")]
    public string RegistrationIndexUrl { get; set; }

    [JsonPropertyName("@type")]
    public IReadOnlyList<string> Type { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    #endregion Original properties from RegistrationIndexResponse.

    [JsonPropertyName("items")]
    public IReadOnlyList<BaGetRegistrationIndexPage> Pages { get; set; }

    [JsonPropertyName("totalDownloads")]
    public long TotalDownloads { get; set; }
}