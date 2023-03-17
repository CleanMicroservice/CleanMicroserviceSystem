using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

public class RegistrationIndexPageItem
{
    [JsonPropertyName("@id")]
    public string RegistrationLeafUrl { get; set; }

    [JsonPropertyName("catalogEntry")]
    public PackageMetadata PackageMetadata { get; set; }

    [JsonPropertyName("packageContent")]
    public string PackageContentUrl { get; set; }
}