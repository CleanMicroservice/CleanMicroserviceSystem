using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core;

public interface IUrlGenerator
{
    string GetServiceIndexUrl();

    string GetPackageContentResourceUrl();

    string GetPackageMetadataResourceUrl();

    string GetPackagePublishResourceUrl();

    string GetSymbolPublishResourceUrl();

    string GetSearchResourceUrl();

    string GetAutocompleteResourceUrl();

    string GetRegistrationIndexUrl(string id);

    string GetRegistrationPageUrl(string id, NuGetVersion lower, NuGetVersion upper);

    string GetRegistrationLeafUrl(string id, NuGetVersion version);

    string GetPackageVersionsUrl(string id);

    string GetPackageDownloadUrl(string id, NuGetVersion version);

    string GetPackageManifestDownloadUrl(string id, NuGetVersion version);

    string GetPackageIconDownloadUrl(string id, NuGetVersion version);
}