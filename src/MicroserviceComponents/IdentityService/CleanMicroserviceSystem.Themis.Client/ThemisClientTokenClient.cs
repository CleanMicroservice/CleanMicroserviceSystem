using System.Net.Http.Json;
using CleanMicroserviceSystem.DataStructure;
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

    public async Task<CommonResult<string>?> LoginClientAsync(string clientName, string secret)
    {
        var uri = this.BuildUri("/api/ClientToken");
        var request = new ClientTokenLoginRequest() { Name = clientName, Secret = secret };
        var response = await this.httpClient.PostAsJsonAsync(uri, request);
        return await this.GetCommonResult<string>(response);
    }

    public async Task<CommonResult<string>?> RefreshClientTokenAsync()
    {
        var uri = this.BuildUri("/api/ClientToken");
        var response = await this.httpClient.PutAsync(uri, null);
        return await this.GetCommonResult<string>(response);
    }

    public async Task<CommonResult?> LogoutClientAsync()
    {
        var uri = this.BuildUri("/api/ClientToken");
        var response = await this.httpClient.DeleteAsync(uri);
        return await this.GetCommonResult(response);
    }
}
