using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.PackageMetadata;

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