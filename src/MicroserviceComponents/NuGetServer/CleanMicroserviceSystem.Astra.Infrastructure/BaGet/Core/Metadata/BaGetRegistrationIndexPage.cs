using System.Text.Json.Serialization;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Metadata;

public class BaGetRegistrationIndexPage
{
    #region Original properties from RegistrationIndexPage.

    [JsonPropertyName("@id")]
    public string RegistrationPageUrl { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("lower")]
    public string Lower { get; set; }

    [JsonPropertyName("upper")]
    public string Upper { get; set; }

    #endregion Original properties from RegistrationIndexPage.

    [JsonPropertyName("items")]
    public IReadOnlyList<BaGetRegistrationIndexPageItem> ItemsOrNull { get; set; }
}