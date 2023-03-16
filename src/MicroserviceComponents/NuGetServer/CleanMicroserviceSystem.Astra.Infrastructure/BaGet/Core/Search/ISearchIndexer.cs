using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Search;

public interface ISearchIndexer
{
    Task IndexAsync(Package package, CancellationToken cancellationToken);
}