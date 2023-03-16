using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Indexing;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Upstream;
using Microsoft.Extensions.Logging;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core;

public class PackageService : IPackageService
{
    private readonly IPackageDatabase _db;
    private readonly IUpstreamClient _upstream;
    private readonly IPackageIndexingService _indexer;
    private readonly ILogger<PackageService> _logger;

    public PackageService(
        IPackageDatabase db,
        IUpstreamClient upstream,
        IPackageIndexingService indexer,
        ILogger<PackageService> logger)
    {
        this._db = db ?? throw new ArgumentNullException(nameof(db));
        this._upstream = upstream ?? throw new ArgumentNullException(nameof(upstream));
        this._indexer = indexer ?? throw new ArgumentNullException(nameof(indexer));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IReadOnlyList<NuGetVersion>> FindPackageVersionsAsync(
        string id,
        CancellationToken cancellationToken)
    {
        var upstreamVersions = await this._upstream.ListPackageVersionsAsync(id, cancellationToken);

        var localPackages = await this._db.FindAsync(id, includeUnlisted: true, cancellationToken);
        var localVersions = localPackages.Select(p => p.Version);

        if (!upstreamVersions.Any()) return localVersions.ToList();
        return !localPackages.Any() ? upstreamVersions : upstreamVersions.Concat(localVersions).Distinct().ToList();
    }

    public async Task<IReadOnlyList<Package>> FindPackagesAsync(string id, CancellationToken cancellationToken)
    {
        var upstreamPackages = await this._upstream.ListPackagesAsync(id, cancellationToken);
        var localPackages = await this._db.FindAsync(id, includeUnlisted: true, cancellationToken);

        if (!upstreamPackages.Any()) return localPackages;
        if (!localPackages.Any()) return upstreamPackages;

        var result = upstreamPackages.ToDictionary(p => p.Version);
        var local = localPackages.ToDictionary(p => p.Version);

        foreach (var localPackage in local)
        {
            result[localPackage.Key] = localPackage.Value;
        }

        return result.Values.ToList();
    }

    public async Task<Package> FindPackageOrNullAsync(
        string id,
        NuGetVersion version,
        CancellationToken cancellationToken)
    {
        return !await this.MirrorAsync(id, version, cancellationToken)
            ? null
            : await this._db.FindOrNullAsync(id, version, includeUnlisted: true, cancellationToken);
    }

    public async Task<bool> ExistsAsync(string id, NuGetVersion version, CancellationToken cancellationToken)
    {
        return await this.MirrorAsync(id, version, cancellationToken);
    }

    public async Task AddDownloadAsync(string packageId, NuGetVersion version, CancellationToken cancellationToken)
    {
        await this._db.AddDownloadAsync(packageId, version, cancellationToken);
    }

    private async Task<bool> MirrorAsync(string id, NuGetVersion version, CancellationToken cancellationToken)
    {
        if (await this._db.ExistsAsync(id, version, cancellationToken))
            return true;

        this._logger.LogInformation(
            "Package {PackageId} {PackageVersion} does not exist locally. Checking upstream feed...",
            id,
            version);

        try
        {
            using var packageStream = await this._upstream.DownloadPackageOrNullAsync(id, version, cancellationToken);
            if (packageStream == null)
            {
                this._logger.LogWarning(
                    "Upstream feed does not have package {PackageId} {PackageVersion}",
                    id,
                    version);
                return false;
            }

            this._logger.LogInformation(
                "Downloaded package {PackageId} {PackageVersion}, indexing...",
                id,
                version);

            var result = await this._indexer.IndexAsync(packageStream, cancellationToken);

            this._logger.LogInformation(
                "Finished indexing package {PackageId} {PackageVersion} from upstream feed with result {Result}",
                id,
                version,
                result);

            return result == PackageIndexingResult.Success;
        }
        catch (Exception e)
        {
            this._logger.LogError(
                e,
                "Failed to index package {PackageId} {PackageVersion} from upstream",
                id,
                version);

            return false;
        }
    }
}