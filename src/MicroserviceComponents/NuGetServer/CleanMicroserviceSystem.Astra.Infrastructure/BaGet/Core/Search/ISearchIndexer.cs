using System.Threading;
using System.Threading.Tasks;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Search
{
    public interface ISearchIndexer
    {
        /// <summary>
        /// Add a package to the search index.
        /// </summary>
        /// <param name="package">The package to add.</param>
        /// <param name="cancellationToken">A token to cancel the task.</param>
        /// <returns>A task that completes once the package has been added.</returns>
        Task IndexAsync(Package package, CancellationToken cancellationToken);
    }
}
