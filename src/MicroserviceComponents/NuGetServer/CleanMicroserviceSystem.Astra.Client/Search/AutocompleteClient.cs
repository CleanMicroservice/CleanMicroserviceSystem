using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

namespace CleanMicroserviceSystem.Astra.Search;

public partial class AstraNuGetClientFactory
{
    private class AutocompleteClient : IAutocompleteClient
    {
        private readonly AstraNuGetClientFactory _clientfactory;

        public AutocompleteClient(AstraNuGetClientFactory clientFactory)
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