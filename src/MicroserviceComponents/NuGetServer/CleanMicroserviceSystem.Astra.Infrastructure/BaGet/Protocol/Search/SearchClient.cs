using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Search;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol;

public partial class NuGetClientFactory
{
    private class SearchClient : ISearchClient
    {
        private readonly NuGetClientFactory _clientfactory;

        public SearchClient(NuGetClientFactory clientFactory)
        {
            this._clientfactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public async Task<SearchResponse> SearchAsync(
            string query = null,
            int skip = 0,
            int take = 20,
            bool includePrerelease = true,
            bool includeSemVer2 = true,
            CancellationToken cancellationToken = default)
        {
            var client = await this._clientfactory.GetSearchClientAsync(cancellationToken);

            return await client.SearchAsync(query, skip, take, includePrerelease, includeSemVer2);
        }
    }
}