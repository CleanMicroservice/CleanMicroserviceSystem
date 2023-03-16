using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Search;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol;

public partial class NuGetClientFactory
{
    private class AutocompleteClient : IAutocompleteClient
    {
        private readonly NuGetClientFactory _clientfactory;

        public AutocompleteClient(NuGetClientFactory clientFactory)
        {
            this._clientfactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public async Task<AutocompleteResponse> AutocompleteAsync(
            string query = null,
            int skip = 0,
            int take = 20,
            bool includePrerelease = true,
            bool includeSemVer2 = true,
            CancellationToken cancellationToken = default)
        {
            var client = await this._clientfactory.GetAutocompleteClientAsync(cancellationToken);

            return await client.AutocompleteAsync(query, skip, take, includePrerelease, includeSemVer2, cancellationToken);
        }

        public async Task<AutocompleteResponse> ListPackageVersionsAsync(
            string packageId,
            bool includePrerelease = true,
            bool includeSemVer2 = true,
            CancellationToken cancellationToken = default)
        {
            var client = await this._clientfactory.GetAutocompleteClientAsync(cancellationToken);

            return await client.ListPackageVersionsAsync(packageId, includePrerelease, includeSemVer2, cancellationToken);
        }
    }
}