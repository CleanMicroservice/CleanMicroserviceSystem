using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

namespace CleanMicroserviceSystem.Astra.Client.PackageMetadata;

public interface IPackageMetadataClient
{
    Task<RegistrationIndexResponse> GetRegistrationIndexOrNullAsync(string packageId, CancellationToken cancellationToken = default);

    Task<RegistrationPageResponse> GetRegistrationPageAsync(
        string pageUrl,
        CancellationToken cancellationToken = default);

    Task<RegistrationLeafResponse> GetRegistrationLeafAsync(
        string leafUrl,
        CancellationToken cancellationToken = default);
}