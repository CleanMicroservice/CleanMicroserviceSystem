using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Upstream;

public interface IUpstreamClient
{
    Task<IReadOnlyList<NuGetVersion>> ListPackageVersionsAsync(string id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Package>> ListPackagesAsync(string id, CancellationToken cancellationToken);

    Task<Stream> DownloadPackageOrNullAsync(string id, NuGetVersion version, CancellationToken cancellationToken);
}