using CleanMicroserviceSystem.Astra.Client.Search;
using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

namespace CleanMicroserviceSystem.Astra.Search;

public partial class AstraNuGetClientFactory
{
    private class SearchClient : ISearchClient
    {
        private readonly AstraNuGetClientFactory _clientfactory;

        public SearchClient(AstraNuGetClientFactory clientFactory)
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