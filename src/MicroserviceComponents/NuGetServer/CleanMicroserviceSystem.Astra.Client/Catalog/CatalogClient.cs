using CleanMicroserviceSystem.Astra.Client.Catalog;
using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

namespace CleanMicroserviceSystem.Astra.Client.Catalog;

public partial class AstraNuGetClientFactory
{
    private class CatalogClient : ICatalogClient
    {
        private readonly AstraNuGetClientFactory _clientfactory;

        public CatalogClient(AstraNuGetClientFactory clientFactory)
        {
            this._clientfactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public async Task<CatalogIndex> GetIndexAsync(CancellationToken cancellationToken = default)
        {
            var client = await this._clientfactory.GetCatalogClientAsync(cancellationToken);

            return await client.GetIndexAsync(cancellationToken);
        }

        public async Task<CatalogPage> GetPageAsync(string pageUrl, CancellationToken cancellationToken = default)
        {
            var client = await this._clientfactory.GetCatalogClientAsync(cancellationToken);

            return await client.GetPageAsync(pageUrl, cancellationToken);
        }

        public async Task<PackageDetailsCatalogLeaf> GetPackageDetailsLeafAsync(string leafUrl, CancellationToken cancellationToken = default)
        {
            var client = await this._clientfactory.GetCatalogClientAsync(cancellationToken);

            return await client.GetPackageDetailsLeafAsync(leafUrl, cancellationToken);
        }

        public async Task<PackageDeleteCatalogLeaf> GetPackageDeleteLeafAsync(string leafUrl, CancellationToken cancellationToken = default)
        {
            var client = await this._clientfactory.GetCatalogClientAsync(cancellationToken);

            return await client.GetPackageDeleteLeafAsync(leafUrl, cancellationToken);
        }
    }
}