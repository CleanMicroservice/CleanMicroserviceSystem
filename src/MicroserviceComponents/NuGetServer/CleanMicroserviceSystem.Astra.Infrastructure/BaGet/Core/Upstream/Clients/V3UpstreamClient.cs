using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Extensions;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Extensions;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;
using Microsoft.Extensions.Logging;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Upstream.Clients;

public class V3UpstreamClient : IUpstreamClient
{
    private readonly NuGetClient _client;
    private readonly ILogger<V3UpstreamClient> _logger;

    public V3UpstreamClient(NuGetClient client, ILogger<V3UpstreamClient> logger)
    {
        this._client = client ?? throw new ArgumentNullException(nameof(client));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Stream> DownloadPackageOrNullAsync(
        string id,
        NuGetVersion version,
        CancellationToken cancellationToken)
    {
        try
        {
            using var downloadStream = await this._client.DownloadPackageAsync(id, version, cancellationToken);
            return await downloadStream.AsTemporaryFileStreamAsync(cancellationToken);
        }
        catch (PackageNotFoundException)
        {
            return null;
        }
        catch (Exception e)
        {
            this._logger.LogError(
                e,
                "Failed to download {PackageId} {PackageVersion} from upstream",
                id,
                version);
            return null;
        }
    }

    public async Task<IReadOnlyList<Package>> ListPackagesAsync(
        string id,
        CancellationToken cancellationToken)
    {
        try
        {
            var packages = await this._client.GetPackageMetadataAsync(id, cancellationToken);

            return packages.Select(this.ToPackage).ToList();
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Failed to mirror {PackageId}'s upstream metadata", id);
            return new List<Package>();
        }
    }

    public async Task<IReadOnlyList<NuGetVersion>> ListPackageVersionsAsync(
        string id,
        CancellationToken cancellationToken)
    {
        try
        {
            return await this._client.ListPackageVersionsAsync(id, includeUnlisted: true, cancellationToken);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Failed to mirror {PackageId}'s upstream versions", id);
            return new List<NuGetVersion>();
        }
    }

    private Package ToPackage(PackageMetadata metadata)
    {
        var version = metadata.ParseVersion();

        return new Package
        {
            Id = metadata.PackageId,
            Version = version,
            Authors = this.ParseAuthors(metadata.Authors),
            Description = metadata.Description,
            Downloads = 0,
            HasReadme = false,
            IsPrerelease = version.IsPrerelease,
            Language = metadata.Language,
            Listed = metadata.IsListed(),
            MinClientVersion = metadata.MinClientVersion,
            Published = metadata.Published.UtcDateTime,
            RequireLicenseAcceptance = metadata.RequireLicenseAcceptance,
            Summary = metadata.Summary,
            Title = metadata.Title,
            IconUrl = this.ParseUri(metadata.IconUrl),
            LicenseUrl = this.ParseUri(metadata.LicenseUrl),
            ProjectUrl = this.ParseUri(metadata.ProjectUrl),
            PackageTypes = new List<PackageType>(),
            RepositoryUrl = null,
            RepositoryType = null,
            SemVerLevel = version.IsSemVer2 ? SemVerLevel.SemVer2 : SemVerLevel.Unknown,
            Tags = metadata.Tags?.ToArray() ?? Array.Empty<string>(),

            Dependencies = this.ToDependencies(metadata)
        };
    }

    private Uri ParseUri(string uriString)
    {
        if (uriString == null) return null;

        return !Uri.TryCreate(uriString, UriKind.Absolute, out var uri) ? null : uri;
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

    private List<PackageDependency> ToDependencies(PackageMetadata package)
    {
        return (package.DependencyGroups?.Count ?? 0) == 0
            ? new List<PackageDependency>()
            : package.DependencyGroups
            .SelectMany(this.ToDependencies)
            .ToList();
    }

    private IEnumerable<PackageDependency> ToDependencies(DependencyGroupItem group)
    {
        return (group.Dependencies?.Count ?? 0) == 0
            ? (IEnumerable<PackageDependency>)(new[]
            {
                new PackageDependency
                {
                    Id = null,
                    VersionRange = null,
                    TargetFramework = group.TargetFramework,
                }
            })
            : group.Dependencies.Select(d => new PackageDependency
            {
                Id = d.Id,
                VersionRange = d.Range,
                TargetFramework = group.TargetFramework,
            });
    }
}