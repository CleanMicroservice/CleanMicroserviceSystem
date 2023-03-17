using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Client.PackageContent;

public partial class AstraNuGetClientFactory
{
    private class PackageContentClient : IPackageContentClient
    {
        private readonly AstraNuGetClientFactory _clientfactory;

        public PackageContentClient(AstraNuGetClientFactory clientFactory)
        {
            this._clientfactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public async Task<Stream> DownloadPackageOrNullAsync(
            string packageId,
            NuGetVersion packageVersion,
            CancellationToken cancellationToken = default)
        {
            var client = await this._clientfactory.GetPackageContentClientAsync(cancellationToken);

            return await client.DownloadPackageOrNullAsync(packageId, packageVersion, cancellationToken);
        }

        public async Task<Stream> DownloadPackageManifestOrNullAsync(
            string packageId,
            NuGetVersion packageVersion,
            CancellationToken cancellationToken = default)
        {
            var client = await this._clientfactory.GetPackageContentClientAsync(cancellationToken);

            return await client.DownloadPackageManifestOrNullAsync(packageId, packageVersion, cancellationToken);
        }

        public async Task<PackageVersionsResponse> GetPackageVersionsOrNullAsync(
            string packageId,
            CancellationToken cancellationToken = default)
        {
            var client = await this._clientfactory.GetPackageContentClientAsync(cancellationToken);

            return await client.GetPackageVersionsOrNullAsync(packageId, cancellationToken);
        }
    }
}