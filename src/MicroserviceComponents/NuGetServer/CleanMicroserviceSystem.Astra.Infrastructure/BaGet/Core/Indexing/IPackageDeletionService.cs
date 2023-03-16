using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Indexing;

public interface IPackageDeletionService
{
    Task<bool> TryDeletePackageAsync(string id, NuGetVersion version, CancellationToken cancellationToken);
}