using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Storage;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core;

public interface IPackageDatabase
{
    Task<PackageAddResult> AddAsync(Package package, CancellationToken cancellationToken);

    Task<Package> FindOrNullAsync(
        string id,
        NuGetVersion version,
        bool includeUnlisted,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<Package>> FindAsync(string id, bool includeUnlisted, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(string id, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(string id, NuGetVersion version, CancellationToken cancellationToken);

    Task<bool> UnlistPackageAsync(string id, NuGetVersion version, CancellationToken cancellationToken);

    Task<bool> RelistPackageAsync(string id, NuGetVersion version, CancellationToken cancellationToken);

    Task AddDownloadAsync(string id, NuGetVersion version, CancellationToken cancellationToken);

    Task<bool> HardDeletePackageAsync(string id, NuGetVersion version, CancellationToken cancellationToken);
}

public enum PackageAddResult
{
    PackageAlreadyExists,
    Success
}