using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Catalog;

public interface ICatalogClient
{
    Task<CatalogIndex> GetIndexAsync(CancellationToken cancellationToken = default);

    Task<CatalogPage> GetPageAsync(
        string pageUrl,
        CancellationToken cancellationToken = default);

    Task<PackageDeleteCatalogLeaf> GetPackageDeleteLeafAsync(
        string leafUrl,
        CancellationToken cancellationToken = default);

    Task<PackageDetailsCatalogLeaf> GetPackageDetailsLeafAsync(
        string leafUrl,
        CancellationToken cancellationToken = default);
}