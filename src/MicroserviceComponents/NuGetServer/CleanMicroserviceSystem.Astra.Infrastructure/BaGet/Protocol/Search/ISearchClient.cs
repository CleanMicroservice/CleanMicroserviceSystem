using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Search;

public interface ISearchClient
{
    Task<SearchResponse> SearchAsync(
        string query = null,
        int skip = 0,
        int take = 20,
        bool includePrerelease = true,
        bool includeSemVer2 = true,
        CancellationToken cancellationToken = default);
}