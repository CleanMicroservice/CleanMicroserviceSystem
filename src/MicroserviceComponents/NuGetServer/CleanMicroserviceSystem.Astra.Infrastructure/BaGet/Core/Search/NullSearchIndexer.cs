using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Search;

public class NullSearchIndexer : ISearchIndexer
{
    public Task IndexAsync(Package package, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}