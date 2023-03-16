using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Upstream.Clients;

public class DisabledUpstreamClient : IUpstreamClient
{
    private readonly IReadOnlyList<NuGetVersion> _emptyVersionList = new List<NuGetVersion>();
    private readonly IReadOnlyList<Package> _emptyPackageList = new List<Package>();

    public Task<IReadOnlyList<NuGetVersion>> ListPackageVersionsAsync(string id, CancellationToken cancellationToken)
    {
        return Task.FromResult(this._emptyVersionList);
    }

    public Task<IReadOnlyList<Package>> ListPackagesAsync(string id, CancellationToken cancellationToken)
    {
        return Task.FromResult(this._emptyPackageList);
    }

    public Task<Stream> DownloadPackageOrNullAsync(
        string id,
        NuGetVersion version,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<Stream>(null);
    }
}