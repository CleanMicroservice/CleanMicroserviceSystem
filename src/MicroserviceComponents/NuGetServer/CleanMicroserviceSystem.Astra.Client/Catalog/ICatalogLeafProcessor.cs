using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

namespace CleanMicroserviceSystem.Astra.Client.Catalog;

public interface ICatalogLeafProcessor
{
    Task<bool> ProcessPackageDetailsAsync(PackageDetailsCatalogLeaf leaf, CancellationToken cancellationToken = default);

    Task<bool> ProcessPackageDeleteAsync(PackageDeleteCatalogLeaf leaf, CancellationToken cancellationToken = default);
}