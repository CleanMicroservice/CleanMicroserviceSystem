using System.Net.Http.Json;
using CleanMicroserviceSystem.Oceanus.Client.Abstraction;
using CleanMicroserviceSystem.Themis.Contract.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanMicroserviceSystem.Themis.Client;

public class ThemisClientTokenClient : OceanusServiceClientBase
{
    public ThemisClientTokenClient(
        ILogger<ThemisClientTokenClient> logger,
        IHttpClientFactory httpClientFactory,
        IOptionsSnapshot<ThemisClientConfiguration> options) :
        base(logger, httpClientFactory, options.Value.GatewayClientName, ThemisClientContract.ThemisUriPrefix)
    {
    }

    public async Task<HttpResponseMessage> LoginClientAsync(string clientName, string secret)
    {
        var uri = this.BuildUri("/api/ClientToken");
        var request = new ClientTokenLoginRequest() { Name = clientName, Secret = secret };
        var response = await this.httpClient.PostAsJsonAsync(uri, request);
        return response;
    }

    public async Task<HttpResponseMessage> RefreshClientTokenAsync()
    {
        var uri = this.BuildUri("/api/ClientToken");
        var response = await this.httpClient.PutAsync(uri, null);
        return response;
    }

    public async Task<HttpResponseMessage> LogoutClientAsync()
    {
        var uri = this.BuildUri("/api/ClientToken");
        var response = await this.httpClient.DeleteAsync(uri);
        return response;
    }
}
