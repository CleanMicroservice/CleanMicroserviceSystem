using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Configuration;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Indexing;

public class PackageDeletionService : IPackageDeletionService
{
    private readonly IPackageDatabase _packages;
    private readonly IPackageStorageService _storage;
    private readonly BaGetOptions _options;
    private readonly ILogger<PackageDeletionService> _logger;

    public PackageDeletionService(
        IPackageDatabase packages,
        IPackageStorageService storage,
        IOptionsSnapshot<BaGetOptions> options,
        ILogger<PackageDeletionService> logger)
    {
        this._packages = packages ?? throw new ArgumentNullException(nameof(packages));
        this._storage = storage ?? throw new ArgumentNullException(nameof(storage));
        this._options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<bool> TryDeletePackageAsync(string id, NuGetVersion version, CancellationToken cancellationToken)
    {
        return this._options.PackageDeletionBehavior switch
        {
            PackageDeletionBehavior.Unlist => await this.TryUnlistPackageAsync(id, version, cancellationToken),
            PackageDeletionBehavior.HardDelete => await this.TryHardDeletePackageAsync(id, version, cancellationToken),
            _ => throw new InvalidOperationException($"Unknown deletion behavior '{this._options.PackageDeletionBehavior}'"),
        };
    }

    private async Task<bool> TryUnlistPackageAsync(string id, NuGetVersion version, CancellationToken cancellationToken)
    {
        this._logger.LogInformation("Unlisting package {PackageId} {PackageVersion}...", id, version);

        if (!await this._packages.UnlistPackageAsync(id, version, cancellationToken))
        {
            this._logger.LogWarning("Could not find package {PackageId} {PackageVersion}", id, version);

            return false;
        }

        this._logger.LogInformation("Unlisted package {PackageId} {PackageVersion}", id, version);

        return true;
    }

    private async Task<bool> TryHardDeletePackageAsync(string id, NuGetVersion version, CancellationToken cancellationToken)
    {
        this._logger.LogInformation(
            "Hard deleting package {PackageId} {PackageVersion} from the database...",
            id,
            version);

        var found = await this._packages.HardDeletePackageAsync(id, version, cancellationToken);
        if (!found)
        {
            this._logger.LogWarning(
                "Could not find package {PackageId} {PackageVersion} in the database",
                id,
                version);
        }

        this._logger.LogInformation("Hard deleting package {PackageId} {PackageVersion} from storage...",
            id,
            version);

        await this._storage.DeleteAsync(id, version, cancellationToken);

        this._logger.LogInformation(
            "Hard deleted package {PackageId} {PackageVersion} from storage",
            id,
            version);

        return found;
    }
}