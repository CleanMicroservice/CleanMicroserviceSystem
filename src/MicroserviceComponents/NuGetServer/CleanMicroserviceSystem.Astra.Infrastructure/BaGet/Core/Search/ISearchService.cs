using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Search;

public interface ISearchService
{
    Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken);

    Task<AutocompleteResponse> AutocompleteAsync(AutocompleteRequest request, CancellationToken cancellationToken);

    Task<AutocompleteResponse> ListPackageVersionsAsync(VersionsRequest request, CancellationToken cancellationToken);

    Task<DependentsResponse> FindDependentsAsync(
        string packageId,
        CancellationToken cancellationToken);
}