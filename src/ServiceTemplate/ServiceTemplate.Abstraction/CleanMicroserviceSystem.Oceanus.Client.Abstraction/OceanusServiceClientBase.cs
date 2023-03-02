using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanMicroserviceSystem.Oceanus.Client.Abstraction;

public abstract class OceanusServiceClientBase
{
    private readonly ILogger<OceanusServiceClientBase> logger;
    private readonly HttpClient httpClient;

    public OceanusServiceClientBase(
        ILogger<OceanusServiceClientBase> logger,
        IOptionsSnapshot<OceanusServiceClientConfiguration> options,
        IHttpClientFactory httpClientFactory)
    {
        this.logger = logger;
        this.httpClient = httpClientFactory.CreateClient(options.Value.GatewayClientName);
    }
}
