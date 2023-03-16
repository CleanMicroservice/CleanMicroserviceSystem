using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Extensions;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Search;

public class RawAutocompleteClient : IAutocompleteClient
{
    private readonly HttpClient _httpClient;
    private readonly string _autocompleteUrl;

    public RawAutocompleteClient(HttpClient httpClient, string autocompleteUrl)
    {
        this._httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this._autocompleteUrl = autocompleteUrl ?? throw new ArgumentNullException(nameof(autocompleteUrl));
    }

    public async Task<AutocompleteResponse> AutocompleteAsync(
        string query = null,
        int skip = 0,
        int take = 20,
        bool includePrerelease = true,
        bool includeSemVer2 = true,
        CancellationToken cancellationToken = default)
    {
        var url = RawSearchClient.AddSearchQueryString(
            this._autocompleteUrl,
            query,
            skip,
            take,
            includePrerelease,
            includeSemVer2,
            "q");

        return await this._httpClient.GetFromJsonAsync<AutocompleteResponse>(url, cancellationToken);
    }

    public async Task<AutocompleteResponse> ListPackageVersionsAsync(
        string packageId,
        bool includePrerelease = true,
        bool includeSemVer2 = true,
        CancellationToken cancellationToken = default)
    {
        var url = RawSearchClient.AddSearchQueryString(
            this._autocompleteUrl,
            packageId,
            skip: null,
            take: null,
            includePrerelease,
            includeSemVer2,
            "id");

        return await this._httpClient.GetFromJsonAsync<AutocompleteResponse>(url, cancellationToken);
    }
}