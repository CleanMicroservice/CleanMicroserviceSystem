using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Upstream;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core;

public interface IPackageService
{
    Task<IReadOnlyList<NuGetVersion>> FindPackageVersionsAsync(string id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Package>> FindPackagesAsync(string id, CancellationToken cancellationToken);

    Task<Package> FindPackageOrNullAsync(string id, NuGetVersion version, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(string id, NuGetVersion version, CancellationToken cancellationToken);

    Task AddDownloadAsync(string packageId, NuGetVersion version, CancellationToken cancellationToken);
}