using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

public class RegistrationLeafResponse
{
    public static readonly IReadOnlyList<string> DefaultType = new List<string>
    {
        "Package",
    };

    [JsonPropertyName("@id")]
    public string RegistrationLeafUrl { get; set; }

    [JsonPropertyName("@type")]
    public IReadOnlyList<string> Type { get; set; }

    [JsonPropertyName("listed")]
    public bool Listed { get; set; }

    [JsonPropertyName("packageContent")]
    public string PackageContentUrl { get; set; }

    [JsonPropertyName("published")]
    public DateTimeOffset Published { get; set; }

    [JsonPropertyName("registration")]
    public string RegistrationIndexUrl { get; set; }
}