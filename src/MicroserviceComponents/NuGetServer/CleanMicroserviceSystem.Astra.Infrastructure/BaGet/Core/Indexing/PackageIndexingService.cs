using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Configuration;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Extensions;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Search;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NuGet.Packaging;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Indexing;

public class PackageIndexingService : IPackageIndexingService
{
    private readonly IPackageDatabase _packages;
    private readonly IPackageStorageService _storage;
    private readonly ISearchIndexer _search;
    private readonly SystemTime _time;
    private readonly IOptionsSnapshot<BaGetOptions> _options;
    private readonly ILogger<PackageIndexingService> _logger;

    public PackageIndexingService(
        IPackageDatabase packages,
        IPackageStorageService storage,
        ISearchIndexer search,
        SystemTime time,
        IOptionsSnapshot<BaGetOptions> options,
        ILogger<PackageIndexingService> logger)
    {
        this._packages = packages ?? throw new ArgumentNullException(nameof(packages));
        this._storage = storage ?? throw new ArgumentNullException(nameof(storage));
        this._search = search ?? throw new ArgumentNullException(nameof(search));
        this._time = time ?? throw new ArgumentNullException(nameof(time));
        this._options = options ?? throw new ArgumentNullException(nameof(options));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<PackageIndexingResult> IndexAsync(Stream packageStream, CancellationToken cancellationToken)
    {
        Package package;
        Stream nuspecStream;
        Stream readmeStream;
        Stream iconStream;

        try
        {
            using var packageReader = new PackageArchiveReader(packageStream, leaveStreamOpen: true);
            package = packageReader.GetPackageMetadata();
            package.Published = this._time.UtcNow;

            nuspecStream = await packageReader.GetNuspecAsync(cancellationToken);
            nuspecStream = await nuspecStream.AsTemporaryFileStreamAsync(cancellationToken);

            if (package.HasReadme)
            {
                readmeStream = await packageReader.GetReadmeAsync(cancellationToken);
                readmeStream = await readmeStream.AsTemporaryFileStreamAsync(cancellationToken);
            }
            else
            {
                readmeStream = null;
            }

            if (package.HasEmbeddedIcon)
            {
                iconStream = await packageReader.GetIconAsync(cancellationToken);
                iconStream = await iconStream.AsTemporaryFileStreamAsync(cancellationToken);
            }
            else
            {
                iconStream = null;
            }
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Uploaded package is invalid");

            return PackageIndexingResult.InvalidPackage;
        }

        if (await this._packages.ExistsAsync(package.Id, package.Version, cancellationToken))
        {
            if (!this._options.Value.AllowPackageOverwrites)
                return PackageIndexingResult.PackageAlreadyExists;

            _ = await this._packages.HardDeletePackageAsync(package.Id, package.Version, cancellationToken);
            await this._storage.DeleteAsync(package.Id, package.Version, cancellationToken);
        }

        this._logger.LogInformation(
            "Validated package {PackageId} {PackageVersion}, persisting content to storage...",
            package.Id,
            package.NormalizedVersionString);

        try
        {
            packageStream.Position = 0;

            await this._storage.SavePackageContentAsync(
                package,
                packageStream,
                nuspecStream,
                readmeStream,
                iconStream,
                cancellationToken);
        }
        catch (Exception e)
        {
            this._logger.LogError(
                e,
                "Failed to persist package {PackageId} {PackageVersion} content to storage",
                package.Id,
                package.NormalizedVersionString);

            throw;
        }

        this._logger.LogInformation(
            "Persisted package {Id} {Version} content to storage, saving metadata to database...",
            package.Id,
            package.NormalizedVersionString);

        var result = await this._packages.AddAsync(package, cancellationToken);
        if (result == PackageAddResult.PackageAlreadyExists)
        {
            this._logger.LogWarning(
                "Package {Id} {Version} metadata already exists in database",
                package.Id,
                package.NormalizedVersionString);

            return PackageIndexingResult.PackageAlreadyExists;
        }

        if (result != PackageAddResult.Success)
        {
            this._logger.LogError($"Unknown {nameof(PackageAddResult)} value: {{PackageAddResult}}", result);

            throw new InvalidOperationException($"Unknown {nameof(PackageAddResult)} value: {result}");
        }

        this._logger.LogInformation(
            "Successfully persisted package {Id} {Version} metadata to database. Indexing in search...",
            package.Id,
            package.NormalizedVersionString);

        await this._search.IndexAsync(package, cancellationToken);

        this._logger.LogInformation(
            "Successfully indexed package {Id} {Version} in search",
            package.Id,
            package.NormalizedVersionString);

        return PackageIndexingResult.Success;
    }
}