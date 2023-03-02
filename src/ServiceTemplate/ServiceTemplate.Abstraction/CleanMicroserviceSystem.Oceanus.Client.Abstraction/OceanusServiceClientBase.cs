using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Oceanus.Client.Abstraction;

public abstract class OceanusServiceClientBase
{
    private readonly ILogger<OceanusServiceClientBase> logger;
    private readonly HttpClient httpClient;

    public OceanusServiceClientBase(
        ILogger<OceanusServiceClientBase> logger,
        IHttpClientFactory httpClientFactory,
        string serviceClientName)
    {
        this.logger = logger;
        this.httpClient = httpClientFactory.CreateClient(serviceClientName);
    }
}
