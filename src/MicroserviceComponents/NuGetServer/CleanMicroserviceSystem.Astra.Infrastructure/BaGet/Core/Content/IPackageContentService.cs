using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Content;

public interface IPackageContentService
{
    Task<PackageVersionsResponse> GetPackageVersionsOrNullAsync(
        string packageId,
        CancellationToken cancellationToken);

    Task<Stream> GetPackageContentStreamOrNullAsync(
        string packageId,
        NuGetVersion packageVersion,
        CancellationToken cancellationToken);

    Task<Stream> GetPackageManifestStreamOrNullAsync(
        string packageId,
        NuGetVersion packageVersion,
        CancellationToken cancellationToken);

    Task<Stream> GetPackageReadmeStreamOrNullAsync(
        string id,
        NuGetVersion version,
        CancellationToken cancellationToken);

    Task<Stream> GetPackageIconStreamOrNullAsync(
        string id,
        NuGetVersion version,
        CancellationToken cancellationToken);
}