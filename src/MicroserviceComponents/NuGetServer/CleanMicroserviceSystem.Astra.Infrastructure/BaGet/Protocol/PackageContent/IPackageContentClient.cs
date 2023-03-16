using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.PackageContent;

public interface IPackageContentClient
{
    Task<PackageVersionsResponse> GetPackageVersionsOrNullAsync(
        string packageId,
        CancellationToken cancellationToken = default);

    Task<Stream> DownloadPackageOrNullAsync(
        string packageId,
        NuGetVersion packageVersion,
        CancellationToken cancellationToken = default);

    Task<Stream> DownloadPackageManifestOrNullAsync(
        string packageId,
        NuGetVersion packageVersion,
        CancellationToken cancellationToken = default);
}