using CleanMicroserviceSystem.Astra.Client.Extensions;
using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

namespace CleanMicroserviceSystem.Astra.Client.Catalog;

public class RawCatalogClient : ICatalogClient
{
    private readonly HttpClient _httpClient;
    private readonly string _catalogUrl;

    public RawCatalogClient(HttpClient httpClient, string catalogUrl)
    {
        this._httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this._catalogUrl = catalogUrl ?? throw new ArgumentNullException(nameof(catalogUrl));
    }

    public async Task<CatalogIndex> GetIndexAsync(CancellationToken cancellationToken = default)
    {
        return await this._httpClient.GetFromJsonAsync<CatalogIndex>(this._catalogUrl, cancellationToken);
    }

    public async Task<CatalogPage> GetPageAsync(string pageUrl, CancellationToken cancellationToken = default)
    {
        return await this._httpClient.GetFromJsonAsync<CatalogPage>(pageUrl, cancellationToken);
    }

    public async Task<PackageDeleteCatalogLeaf> GetPackageDeleteLeafAsync(string leafUrl, CancellationToken cancellationToken = default)
    {
        return await this.GetAndValidateLeafAsync<PackageDeleteCatalogLeaf>(
            "PackageDelete",
            leafUrl,
            cancellationToken);
    }

    public async Task<PackageDetailsCatalogLeaf> GetPackageDetailsLeafAsync(string leafUrl, CancellationToken cancellationToken = default)
    {
        return await this.GetAndValidateLeafAsync<PackageDetailsCatalogLeaf>(
            "PackageDetails",
            leafUrl,
            cancellationToken);
    }

    private async Task<TCatalogLeaf> GetAndValidateLeafAsync<TCatalogLeaf>(
        string leafType,
        string leafUrl,
        CancellationToken cancellationToken) where TCatalogLeaf : CatalogLeaf
    {
        var leaf = await this._httpClient.GetFromJsonAsync<TCatalogLeaf>(leafUrl, cancellationToken);

        return leaf.Type.FirstOrDefault() != leafType
            ? throw new ArgumentException(
                $"The leaf type found in the document does not match the expected '{leafType}' type.",
                nameof(leafType))
            : leaf;
    }
}