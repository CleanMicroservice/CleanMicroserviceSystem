using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.ServiceIndex;

public class BaGetServiceIndex : IServiceIndexService
{
    private readonly IUrlGenerator _url;

    public BaGetServiceIndex(IUrlGenerator url)
    {
        this._url = url ?? throw new ArgumentNullException(nameof(url));
    }

    private IEnumerable<ServiceIndexItem> BuildResource(string name, string url, params string[] versions)
    {
        foreach (var version in versions)
        {
            var type = string.IsNullOrEmpty(version) ? name : $"{name}/{version}";

            yield return new ServiceIndexItem
            {
                ResourceUrl = url,
                Type = type,
            };
        }
    }

    public Task<ServiceIndexResponse> GetAsync(CancellationToken cancellationToken = default)
    {
        var resources = new List<ServiceIndexItem>();

        resources.AddRange(this.BuildResource("PackagePublish", this._url.GetPackagePublishResourceUrl(), "2.0.0"));
        resources.AddRange(this.BuildResource("SymbolPackagePublish", this._url.GetSymbolPublishResourceUrl(), "4.9.0"));
        resources.AddRange(this.BuildResource("SearchQueryService", this._url.GetSearchResourceUrl(), "", "3.0.0-beta", "3.0.0-rc"));
        resources.AddRange(this.BuildResource("RegistrationsBaseUrl", this._url.GetPackageMetadataResourceUrl(), "", "3.0.0-rc", "3.0.0-beta"));
        resources.AddRange(this.BuildResource("PackageBaseAddress", this._url.GetPackageContentResourceUrl(), "3.0.0"));
        resources.AddRange(this.BuildResource("SearchAutocompleteService", this._url.GetAutocompleteResourceUrl(), "", "3.0.0-rc", "3.0.0-beta"));

        var result = new ServiceIndexResponse
        {
            Version = "3.0.0",
            Resources = resources,
        };

        return Task.FromResult(result);
    }
}