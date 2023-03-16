using System.Text.Json.Serialization;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Metadata;

public class BaGetRegistrationIndexPageItem
{
    #region Original properties from RegistrationIndexPageItem.

    [JsonPropertyName("@id")]
    public string RegistrationLeafUrl { get; set; }

    [JsonPropertyName("packageContent")]
    public string PackageContentUrl { get; set; }

    #endregion Original properties from RegistrationIndexPageItem.

    [JsonPropertyName("catalogEntry")]
    public BaGetPackageMetadata PackageMetadata { get; set; }
}