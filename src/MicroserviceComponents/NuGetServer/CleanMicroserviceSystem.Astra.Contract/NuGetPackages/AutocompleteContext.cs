using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

public class AutocompleteContext
{
    public static readonly AutocompleteContext Default = new()
    {
    };

    [JsonPropertyName("@vocab")]
    public string Vocab { get; set; }
}