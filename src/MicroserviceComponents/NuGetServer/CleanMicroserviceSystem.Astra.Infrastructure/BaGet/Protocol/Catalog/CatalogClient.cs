using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Catalog;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol;

public partial class NuGetClientFactory
{
    private class CatalogClient : ICatalogClient
    {
        private readonly NuGetClientFactory _clientfactory;

        public CatalogClient(NuGetClientFactory clientFactory)
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