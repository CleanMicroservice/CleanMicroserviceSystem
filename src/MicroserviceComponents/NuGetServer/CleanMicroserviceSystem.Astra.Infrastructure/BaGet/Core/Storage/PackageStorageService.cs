using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;
using Microsoft.Extensions.Logging;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Storage;

public class PackageStorageService : IPackageStorageService
{
    private const string PackagesPathPrefix = "packages";

    private const string PackageContentType = "binary/octet-stream";

    private const string NuspecContentType = "text/plain";
    private const string ReadmeContentType = "text/markdown";
    private const string IconContentType = "image/xyz";

    private readonly IStorageService _storage;
    private readonly ILogger<PackageStorageService> _logger;

    public PackageStorageService(
        IStorageService storage,
        ILogger<PackageStorageService> logger)
    {
        this._storage = storage ?? throw new ArgumentNullException(nameof(storage));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SavePackageContentAsync(
        Package package,
        Stream packageStream,
        Stream nuspecStream,
        Stream readmeStream,
        Stream iconStream,
        CancellationToken cancellationToken = default)
    {
        package = package ?? throw new ArgumentNullException(nameof(package));
        packageStream = packageStream ?? throw new ArgumentNullException(nameof(packageStream));
        nuspecStream = nuspecStream ?? throw new ArgumentNullException(nameof(nuspecStream));

        var lowercasedId = package.Id.ToLowerInvariant();
        var lowercasedNormalizedVersion = package.NormalizedVersionString.ToLowerInvariant();

        var packagePath = this.PackagePath(lowercasedId, lowercasedNormalizedVersion);
        var nuspecPath = this.NuspecPath(lowercasedId, lowercasedNormalizedVersion);
        var readmePath = this.ReadmePath(lowercasedId, lowercasedNormalizedVersion);
        var iconPath = this.IconPath(lowercasedId, lowercasedNormalizedVersion);

        this._logger.LogInformation(
            "Storing package {PackageId} {PackageVersion} at {Path}...",
            lowercasedId,
            lowercasedNormalizedVersion,
            packagePath);

        var result = await this._storage.PutAsync(packagePath, packageStream, PackageContentType, cancellationToken);
        if (result == StoragePutResult.Conflict)
        {
            this._logger.LogInformation(
                "Could not store package {PackageId} {PackageVersion} at {Path} due to conflict",
                lowercasedId,
                lowercasedNormalizedVersion,
                packagePath);

            throw new InvalidOperationException($"Failed to store package {lowercasedId} {lowercasedNormalizedVersion} due to conflict");
        }

        this._logger.LogInformation(
            "Storing package {PackageId} {PackageVersion} nuspec at {Path}...",
            lowercasedId,
            lowercasedNormalizedVersion,
            nuspecPath);

        result = await this._storage.PutAsync(nuspecPath, nuspecStream, NuspecContentType, cancellationToken);
        if (result == StoragePutResult.Conflict)
        {
            this._logger.LogInformation(
                "Could not store package {PackageId} {PackageVersion} nuspec at {Path} due to conflict",
                lowercasedId,
                lowercasedNormalizedVersion,
                nuspecPath);

            throw new InvalidOperationException($"Failed to store package {lowercasedId} {lowercasedNormalizedVersion} nuspec due to conflict");
        }

        if (readmeStream != null)
        {
            this._logger.LogInformation(
                "Storing package {PackageId} {PackageVersion} readme at {Path}...",
                lowercasedId,
                lowercasedNormalizedVersion,
                readmePath);

            result = await this._storage.PutAsync(readmePath, readmeStream, ReadmeContentType, cancellationToken);
            if (result == StoragePutResult.Conflict)
            {
                this._logger.LogInformation(
                    "Could not store package {PackageId} {PackageVersion} readme at {Path} due to conflict",
                    lowercasedId,
                    lowercasedNormalizedVersion,
                    readmePath);

                throw new InvalidOperationException($"Failed to store package {lowercasedId} {lowercasedNormalizedVersion} readme due to conflict");
            }
        }

        if (iconStream != null)
        {
            this._logger.LogInformation(
                "Storing package {PackageId} {PackageVersion} icon at {Path}...",
                lowercasedId,
                lowercasedNormalizedVersion,
                iconPath);

            result = await this._storage.PutAsync(iconPath, iconStream, IconContentType, cancellationToken);
            if (result == StoragePutResult.Conflict)
            {
                this._logger.LogInformation(
                    "Could not store package {PackageId} {PackageVersion} icon at {Path} due to conflict",
                    lowercasedId,
                    lowercasedNormalizedVersion,
                    iconPath);

                throw new InvalidOperationException($"Failed to store package {lowercasedId} {lowercasedNormalizedVersion} icon");
            }
        }

        this._logger.LogInformation(
            "Finished storing package {PackageId} {PackageVersion}",
            lowercasedId,
            lowercasedNormalizedVersion);
    }

    public async Task<Stream> GetPackageStreamAsync(string id, NuGetVersion version, CancellationToken cancellationToken)
    {
        return await this.GetStreamAsync(id, version, this.PackagePath, cancellationToken);
    }

    public async Task<Stream> GetNuspecStreamAsync(string id, NuGetVersion version, CancellationToken cancellationToken)
    {
        return await this.GetStreamAsync(id, version, this.NuspecPath, cancellationToken);
    }

    public async Task<Stream> GetReadmeStreamAsync(string id, NuGetVersion version, CancellationToken cancellationToken)
    {
        return await this.GetStreamAsync(id, version, this.ReadmePath, cancellationToken);
    }

    public async Task<Stream> GetIconStreamAsync(string id, NuGetVersion version, CancellationToken cancellationToken)
    {
        return await this.GetStreamAsync(id, version, this.IconPath, cancellationToken);
    }

    public async Task DeleteAsync(string id, NuGetVersion version, CancellationToken cancellationToken)
    {
        var lowercasedId = id.ToLowerInvariant();
        var lowercasedNormalizedVersion = version.ToNormalizedString().ToLowerInvariant();

        var packagePath = this.PackagePath(lowercasedId, lowercasedNormalizedVersion);
        var nuspecPath = this.NuspecPath(lowercasedId, lowercasedNormalizedVersion);
        var readmePath = this.ReadmePath(lowercasedId, lowercasedNormalizedVersion);
        var iconPath = this.IconPath(lowercasedId, lowercasedNormalizedVersion);

        await this._storage.DeleteAsync(packagePath, cancellationToken);
        await this._storage.DeleteAsync(nuspecPath, cancellationToken);
        await this._storage.DeleteAsync(readmePath, cancellationToken);
        await this._storage.DeleteAsync(iconPath, cancellationToken);
    }

    private async Task<Stream> GetStreamAsync(
        string id,
        NuGetVersion version,
        Func<string, string, string> pathFunc,
        CancellationToken cancellationToken)
    {
        var lowercasedId = id.ToLowerInvariant();
        var lowercasedNormalizedVersion = version.ToNormalizedString().ToLowerInvariant();
        var path = pathFunc(lowercasedId, lowercasedNormalizedVersion);

        try
        {
            return await this._storage.GetAsync(path, cancellationToken);
        }
        catch (DirectoryNotFoundException)
        {
            this._logger.LogError(
                $"Unable to find the '{PackagesPathPrefix}' folder. " +
                "If you've recently upgraded BaGet, please make sure this folder starts with a lowercased letter. " +
                "For more information, please see https://github.com/loic-sharma/BaGet/issues/298");
            throw;
        }
    }

    private string PackagePath(string lowercasedId, string lowercasedNormalizedVersion)
    {
        return Path.Combine(
            PackagesPathPrefix,
            lowercasedId,
            lowercasedNormalizedVersion,
            $"{lowercasedId}.{lowercasedNormalizedVersion}.nupkg");
    }

    private string NuspecPath(string lowercasedId, string lowercasedNormalizedVersion)
    {
        return Path.Combine(
            PackagesPathPrefix,
            lowercasedId,
            lowercasedNormalizedVersion,
            $"{lowercasedId}.nuspec");
    }

    private string ReadmePath(string lowercasedId, string lowercasedNormalizedVersion)
    {
        return Path.Combine(
            PackagesPathPrefix,
            lowercasedId,
            lowercasedNormalizedVersion,
            "readme");
    }

    private string IconPath(string lowercasedId, string lowercasedNormalizedVersion)
    {
        return Path.Combine(
            PackagesPathPrefix,
            lowercasedId,
            lowercasedNormalizedVersion,
            "icon");
    }
}