using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Oceanus.Client.Abstraction;

public abstract class OceanusServiceClientBase
{
    protected readonly ILogger<OceanusServiceClientBase> logger;
    protected readonly string serviceUriPrefix;
    protected readonly HttpClient httpClient;
    protected readonly Uri baseUriPrefix;

    public OceanusServiceClientBase(
        ILogger<OceanusServiceClientBase> logger,
        IHttpClientFactory httpClientFactory,
        string serviceClientName,
        string serviceUriPrefix)
    {
        this.logger = logger;
        this.serviceUriPrefix = serviceUriPrefix;
        this.httpClient = httpClientFactory.CreateClient(serviceClientName);
        this.baseUriPrefix = new Uri(this.httpClient.BaseAddress!, serviceUriPrefix);
    }

    protected virtual Uri BuildUri(string uri) => new(this.baseUriPrefix, uri);
}
