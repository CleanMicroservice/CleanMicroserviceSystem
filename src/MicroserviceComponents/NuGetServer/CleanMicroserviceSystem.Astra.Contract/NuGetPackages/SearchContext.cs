using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

public class SearchContext
{
    public static SearchContext Default(string registrationBaseUrl)
    {
        return new SearchContext
        {
            Base = registrationBaseUrl
        };
    }

    [JsonPropertyName("@vocab")]
    public string Vocab { get; set; }

    [JsonPropertyName("@base")]
    public string Base { get; set; }
}