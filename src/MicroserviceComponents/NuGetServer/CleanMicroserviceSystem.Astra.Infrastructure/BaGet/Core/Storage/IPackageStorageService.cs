using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Storage;

public interface IPackageStorageService
{
    Task SavePackageContentAsync(
        Package package,
        Stream packageStream,
        Stream nuspecStream,
        Stream readmeStream,
        Stream iconStream,
        CancellationToken cancellationToken);

    Task<Stream> GetPackageStreamAsync(string id, NuGetVersion version, CancellationToken cancellationToken);

    Task<Stream> GetNuspecStreamAsync(string id, NuGetVersion version, CancellationToken cancellationToken);

    Task<Stream> GetReadmeStreamAsync(string id, NuGetVersion version, CancellationToken cancellationToken);

    Task<Stream> GetIconStreamAsync(string id, NuGetVersion version, CancellationToken cancellationToken);

    Task DeleteAsync(string id, NuGetVersion version, CancellationToken cancellationToken);
}