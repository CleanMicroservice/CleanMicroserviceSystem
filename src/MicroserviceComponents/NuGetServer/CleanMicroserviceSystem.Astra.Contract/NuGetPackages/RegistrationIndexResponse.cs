using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

public class RegistrationIndexResponse
{
    public static readonly IReadOnlyList<string> DefaultType = new List<string>
    {
        "catalog:CatalogRoot",
        "PackageRegistration",
        "catalog:Permalink"
    };

    [JsonPropertyName("@id")]
    public string RegistrationIndexUrl { get; set; }

    [JsonPropertyName("@type")]
    public IReadOnlyList<string> Type { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("items")]
    public IReadOnlyList<RegistrationIndexPage> Pages { get; set; }
}