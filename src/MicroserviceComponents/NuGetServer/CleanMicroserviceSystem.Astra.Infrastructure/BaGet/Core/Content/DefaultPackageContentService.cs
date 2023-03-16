using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Storage;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Content;

public class DefaultPackageContentService : IPackageContentService
{
    private readonly IPackageService _packages;
    private readonly IPackageStorageService _storage;

    public DefaultPackageContentService(
        IPackageService packages,
        IPackageStorageService storage)
    {
        this._packages = packages ?? throw new ArgumentNullException(nameof(packages));
        this._storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }

    public async Task<PackageVersionsResponse> GetPackageVersionsOrNullAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        var versions = await this._packages.FindPackageVersionsAsync(id, cancellationToken);
        return !versions.Any()
            ? null
            : new PackageVersionsResponse
            {
                Versions = versions
                .Select(v => v.ToNormalizedString())
                .Select(v => v.ToLowerInvariant())
                .ToList()
            };
    }

    public async Task<Stream> GetPackageContentStreamOrNullAsync(
        string id,
        NuGetVersion version,
        CancellationToken cancellationToken = default)
    {
        if (!await this._packages.ExistsAsync(id, version, cancellationToken))
            return null;

        await this._packages.AddDownloadAsync(id, version, cancellationToken);
        return await this._storage.GetPackageStreamAsync(id, version, cancellationToken);
    }

    public async Task<Stream> GetPackageManifestStreamOrNullAsync(string id, NuGetVersion version, CancellationToken cancellationToken = default)
    {
        return !await this._packages.ExistsAsync(id, version, cancellationToken)
            ? null
            : await this._storage.GetNuspecStreamAsync(id, version, cancellationToken);
    }

    public async Task<Stream> GetPackageReadmeStreamOrNullAsync(string id, NuGetVersion version, CancellationToken cancellationToken = default)
    {
        var package = await this._packages.FindPackageOrNullAsync(id, version, cancellationToken);
        return package == null || !package.HasReadme ? null : await this._storage.GetReadmeStreamAsync(id, version, cancellationToken);
    }

    public async Task<Stream> GetPackageIconStreamOrNullAsync(string id, NuGetVersion version, CancellationToken cancellationToken = default)
    {
        var package = await this._packages.FindPackageOrNullAsync(id, version, cancellationToken);
        return package == null || !package.HasEmbeddedIcon ? null : await this._storage.GetIconStreamAsync(id, version, cancellationToken);
    }
}