using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Search;

public interface IAutocompleteClient
{
    Task<AutocompleteResponse> AutocompleteAsync(
        string query = null,
        int skip = 0,
        int take = 20,
        bool includePrerelease = true,
        bool includeSemVer2 = true,
        CancellationToken cancellationToken = default);

    Task<AutocompleteResponse> ListPackageVersionsAsync(
        string packageId,
        bool includePrerelease = true,
        bool includeSemVer2 = true,
        CancellationToken cancellationToken = default);
}