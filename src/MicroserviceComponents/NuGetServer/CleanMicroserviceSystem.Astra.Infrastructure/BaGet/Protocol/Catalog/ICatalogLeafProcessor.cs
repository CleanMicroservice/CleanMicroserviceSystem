using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Catalog;

public interface ICatalogLeafProcessor
{
    Task<bool> ProcessPackageDetailsAsync(PackageDetailsCatalogLeaf leaf, CancellationToken cancellationToken = default);

    Task<bool> ProcessPackageDeleteAsync(PackageDeleteCatalogLeaf leaf, CancellationToken cancellationToken = default);
}