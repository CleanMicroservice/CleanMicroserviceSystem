using System.Threading;
using System.Threading.Tasks;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Search
{
    /// <summary>
    /// A no-op indexer, used when search does not need to index packages.
    /// </summary>
    public class NullSearchIndexer : ISearchIndexer
    {
        public Task IndexAsync(Package package, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
