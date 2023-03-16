using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Catalog;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Extensions;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.PackageContent;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.PackageMetadata;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Search;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.ServiceIndex;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol;

/// <summary>
/// The <see cref="NuGetClientFactory"/> creates clients to interact with a NuGet server.
/// Use this for advanced scenarios. For most scenarios, consider using <see cref="NuGetClient"/> instead.
/// </summary>
public partial class NuGetClientFactory
{
    private readonly HttpClient _httpClient;
    private readonly string _serviceIndexUrl;

    private readonly SemaphoreSlim _mutex;
    private NuGetClients _clients;

    /// <summary>
    /// Initializes a new instance of the <see cref="NuGetClientFactory"/> class
    /// for mocking.
    /// </summary>
    protected NuGetClientFactory()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NuGetClientFactory"/> class.
    /// </summary>
    /// <param name="httpClient">The client used for HTTP requests.</param>
    /// <param name="serviceIndexUrl">
    /// The NuGet Service Index resource URL.
    ///
    /// For NuGet.org, use https://api.nuget.org/v3/index.json
    /// </param>
    public NuGetClientFactory(HttpClient httpClient, string serviceIndexUrl)
    {
        this._httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this._serviceIndexUrl = serviceIndexUrl ?? throw new ArgumentNullException(nameof(serviceIndexUrl));

        this._mutex = new SemaphoreSlim(1, 1);
        this._clients = null;
    }

    /// <summary>
    /// Create a client to interact with the NuGet Service Index resource.
    /// 
    /// See https://docs.microsoft.com/en-us/nuget/api/service-index
    /// </summary>
    /// <returns>A client to interact with the NuGet Service Index resource.</returns>
    public virtual IServiceIndexClient CreateServiceIndexClient()
    {
        return new ServiceIndexClient(this);
    }

    /// <summary>
    /// Create a client to interact with the NuGet Package Content resource.
    ///
    /// See https://docs.microsoft.com/en-us/nuget/api/package-base-address-resource
    /// </summary>
    /// <returns>A client to interact with the NuGet Package Content resource.</returns>
    public virtual IPackageContentClient CreatePackageContentClient()
    {
        return new PackageContentClient(this);
    }

    /// <summary>
    /// Create a client to interact with the NuGet Package Metadata resource.
    /// 
    /// See https://docs.microsoft.com/en-us/nuget/api/registration-base-url-resource
    /// </summary>
    /// <returns>A client to interact with the NuGet Package Metadata resource.</returns>
    public virtual IPackageMetadataClient CreatePackageMetadataClient()
    {
        return new PackageMetadataClient(this);
    }

    /// <summary>
    /// Create a client to interact with the NuGet Search resource.
    /// 
    /// See https://docs.microsoft.com/en-us/nuget/api/search-query-service-resource
    /// </summary>
    /// <returns>A client to interact with the NuGet Search resource.</returns>
    public virtual ISearchClient CreateSearchClient()
    {
        return new SearchClient(this);
    }

    /// <summary>
    /// Create a client to interact with the NuGet Autocomplete resource.
    /// 
    /// See https://docs.microsoft.com/en-us/nuget/api/search-autocomplete-service-resource
    /// </summary>
    /// <returns>A client to interact with the NuGet Autocomplete resource.</returns>
    public virtual IAutocompleteClient CreateAutocompleteClient()
    {
        return new AutocompleteClient(this);
    }

    /// <summary>
    /// Create a client to interact with the NuGet catalog resource.
    /// 
    /// See https://docs.microsoft.com/en-us/nuget/api/catalog-resource
    /// </summary>
    /// <returns>A client to interact with the Catalog resource.</returns>
    public virtual ICatalogClient CreateCatalogClient()
    {
        return new CatalogClient(this);
    }

    private Task<ServiceIndexResponse> GetServiceIndexAsync(CancellationToken cancellationToken = default)
    {
        return this.GetAsync(c => c.ServiceIndex, cancellationToken);
    }

    private Task<IPackageContentClient> GetPackageContentClientAsync(CancellationToken cancellationToken = default)
    {
        return this.GetAsync(c => c.PackageContentClient, cancellationToken);
    }

    private Task<IPackageMetadataClient> GetPackageMetadataClientAsync(CancellationToken cancellationToken = default)
    {
        return this.GetAsync(c => c.PackageMetadataClient, cancellationToken);
    }

    private Task<ISearchClient> GetSearchClientAsync(CancellationToken cancellationToken = default)
    {
        return this.GetAsync(c => c.SearchClient, cancellationToken);
    }

    private Task<IAutocompleteClient> GetAutocompleteClientAsync(CancellationToken cancellationToken = default)
    {
        return this.GetAsync(c => c.AutocompleteClient, cancellationToken);
    }

    private Task<ICatalogClient> GetCatalogClientAsync(CancellationToken cancellationToken = default)
    {
        return this.GetAsync(c => c.CatalogClient, cancellationToken);
    }

    private async Task<T> GetAsync<T>(Func<NuGetClients, T> clientFactory, CancellationToken cancellationToken)
    {
        if (this._clients == null)
        {
            await this._mutex.WaitAsync(cancellationToken);

            try
            {
                if (this._clients == null)
                {
                    var serviceIndexClient = new RawServiceIndexClient(this._httpClient, this._serviceIndexUrl);

                    var serviceIndex = await serviceIndexClient.GetAsync(cancellationToken);

                    var contentResourceUrl = serviceIndex.GetPackageContentResourceUrl();
                    var metadataResourceUrl = serviceIndex.GetPackageMetadataResourceUrl();
                    var catalogResourceUrl = serviceIndex.GetCatalogResourceUrl();
                    var searchResourceUrl = serviceIndex.GetSearchQueryResourceUrl();
                    var autocompleteResourceUrl = serviceIndex.GetSearchAutocompleteResourceUrl();

                    // Create clients for required resources.
                    var contentClient = new RawPackageContentClient(this._httpClient, contentResourceUrl);
                    var metadataClient = new RawPackageMetadataClient(this._httpClient, metadataResourceUrl);
                    var searchClient = new RawSearchClient(this._httpClient, searchResourceUrl);

                    // Create clients for optional resources.
                    var catalogClient = catalogResourceUrl == null
                        ? new NullCatalogClient() as ICatalogClient
                        : new RawCatalogClient(this._httpClient, catalogResourceUrl);
                    var autocompleteClient = autocompleteResourceUrl == null
                        ? new NullAutocompleteClient() as IAutocompleteClient
                        : new RawAutocompleteClient(this._httpClient, autocompleteResourceUrl);

                    this._clients = new NuGetClients
                    {
                        ServiceIndex = serviceIndex,

                        PackageContentClient = contentClient,
                        PackageMetadataClient = metadataClient,
                        SearchClient = searchClient,
                        AutocompleteClient = autocompleteClient,
                        CatalogClient = catalogClient,
                    };
                }
            }
            finally
            {
                _ = this._mutex.Release();
            }
        }

        // TODO: This should periodically refresh the service index response.
        return clientFactory(this._clients);
    }

    private class NuGetClients
    {
        public ServiceIndexResponse ServiceIndex { get; set; }

        public IPackageContentClient PackageContentClient { get; set; }
        public IPackageMetadataClient PackageMetadataClient { get; set; }
        public ISearchClient SearchClient { get; set; }
        public IAutocompleteClient AutocompleteClient { get; set; }
        public ICatalogClient CatalogClient { get; set; }
    }
}
