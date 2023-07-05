using System.Net.Http.Json;
using CleanMicroserviceSystem.DataStructure;
using CleanMicroserviceSystem.Hermes.Contract;
using CleanMicroserviceSystem.Oceanus.Client.Abstraction;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanMicroserviceSystem.Hermes.Client;

public class K8sAgentClient : OceanusServiceClientBase
{
    public K8sAgentClient(
        ILogger<K8sAgentClient> logger,
        IHttpClientFactory httpClientFactory,
        IOptionsSnapshot<K8sAgentClientConfiguration> options) :
        base(logger, httpClientFactory, options.Value.GatewayClientName, K8sAgentClientContract.HermesUriPrefix)
    {
    }

    public async Task<CommonResult<string>?> SendAsync(SendParameter parameter)
    {
        var uri = this.BuildUri("/home/index");
        var response = await this.httpClient.PostAsJsonAsync(uri, parameter);
        return await this.GetCommonResult<string>(response);
    }
}
