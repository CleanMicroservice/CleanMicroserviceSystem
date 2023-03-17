using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using ILogger = Microsoft.Extensions.Logging.ILogger<CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Upstream.Clients.V2UpstreamClient>;
using INuGetLogger = NuGet.Common.ILogger;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Upstream.Clients;

public class V2UpstreamClient : IUpstreamClient, IDisposable
{
    private readonly SourceCacheContext _cache;
    private readonly SourceRepository _repository;
    private readonly INuGetLogger _ngLogger;
    private readonly ILogger _logger;

    public V2UpstreamClient(
        IOptionsSnapshot<MirrorOptions> options,
        ILogger logger)
    {
        if (options is null)
            throw new ArgumentNullException(nameof(options));

        if (options.Value?.PackageSource?.AbsolutePath == null)
            throw new ArgumentException("No mirror package source has been set.");

        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));

        this._ngLogger = NullLogger.Instance;
        this._cache = new SourceCacheContext();
        this._repository = Repository.Factory.GetCoreV2(new PackageSource(options.Value.PackageSource.AbsoluteUri));
    }

    public async Task<IReadOnlyList<NuGetVersion>> ListPackageVersionsAsync(string id, CancellationToken cancellationToken)
    {
        try
        {
            var resource = await this._repository.GetResourceAsync<FindPackageByIdResource>(cancellationToken);
            var versions = await resource.GetAllVersionsAsync(id, this._cache, this._ngLogger, cancellationToken);

            return versions.ToList();
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Failed to mirror {PackageId}'s upstream versions", id);
            return new List<NuGetVersion>();
        }
    }

    public async Task<IReadOnlyList<Package>> ListPackagesAsync(
        string id,
        CancellationToken cancellationToken)
    {
        try
        {
            var resource = await this._repository.GetResourceAsync<PackageMetadataResource>(cancellationToken);
            var packages = await resource.GetMetadataAsync(
                id,
                includePrerelease: true,
                includeUnlisted: true,
                this._cache,
                this._ngLogger,
                cancellationToken);

            return packages.Select(this.ToPackage).ToList();
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Failed to mirror {PackageId}'s upstream versions", id);
            return new List<Package>();
        }
    }

    public async Task<Stream> DownloadPackageOrNullAsync(
        string id,
        NuGetVersion version,
        CancellationToken cancellationToken)
    {
        var packageStream = new MemoryStream();

        try
        {
            var resource = await this._repository.GetResourceAsync<FindPackageByIdResource>(cancellationToken);
            var success = await resource.CopyNupkgToStreamAsync(
                id, version, packageStream, this._cache, this._ngLogger,
                cancellationToken);

            if (!success)
            {
                packageStream.Dispose();
                return null;
            }

            packageStream.Position = 0;

            return packageStream;
        }
        catch (Exception e)
        {
            this._logger.LogError(
                e,
                "Failed to index package {Id} {Version} from upstream",
                id,
                version);

            packageStream.Dispose();
            return null;
        }
    }

    public void Dispose()
    {
        this._cache.Dispose();
    }

    private Package ToPackage(IPackageSearchMetadata package)
    {
        return new Package
        {
            Id = package.Identity.Id,
            Version = package.Identity.Version,
            Authors = this.ParseAuthors(package.Authors),
            Description = package.Description,
            Downloads = 0,
            HasReadme = false,
            Language = null,
            Listed = package.IsListed,
            MinClientVersion = null,
            Published = package.Published?.UtcDateTime ?? DateTime.MinValue,
            RequireLicenseAcceptance = package.RequireLicenseAcceptance,
            Summary = package.Summary,
            Title = package.Title,
            IconUrl = package.IconUrl,
            LicenseUrl = package.LicenseUrl,
            ProjectUrl = package.ProjectUrl,
            PackageTypes = new List<PackageType>(),
            RepositoryUrl = null,
            RepositoryType = null,
            Tags = package.Tags?.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries),

            Dependencies = this.ToDependencies(package)
        };
    }

    private string[] ParseAuthors(string authors)
    {
        return string.IsNullOrEmpty(authors)
            ? Array.Empty<string>()
            : authors
            .Split(new[] { ',', ';', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(a => a.Trim())
            .ToArray();
    }

    private List<PackageDependency> ToDependencies(IPackageSearchMetadata package)
    {
        return package
            .DependencySets
            .SelectMany(this.ToDependencies)
            .ToList();
    }

    private IEnumerable<PackageDependency> ToDependencies(PackageDependencyGroup group)
    {
        var framework = group.TargetFramework.GetShortFolderName();

        return (group.Packages?.Count() ?? 0) == 0
            ? (IEnumerable<PackageDependency>)(new[]
            {
                new PackageDependency
                {
                    Id = null,
                    VersionRange = null,
                    TargetFramework = framework,
                }
            })
            : group.Packages.Select(d => new PackageDependency
            {
                Id = d.Id,
                VersionRange = d.VersionRange?.ToString(),
                TargetFramework = framework,
            });
    }
}