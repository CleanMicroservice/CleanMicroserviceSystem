using System.Net.Http.Json;
using CleanMicroserviceSystem.Oceanus.Client.Abstraction;
using CleanMicroserviceSystem.Themis.Contract.Users;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanMicroserviceSystem.Themis.Client;

public class ThemisUserTokenClient : OceanusServiceClientBase
{
    public ThemisUserTokenClient(
        ILogger<ThemisUserTokenClient> logger,
        IHttpClientFactory httpClientFactory,
        IOptionsSnapshot<ThemisClientConfiguration> options) :
        base(logger, httpClientFactory, options.Value.GatewayClientName, ThemisClientContract.ThemisUriPrefix)
    {
    }

    public async Task<HttpResponseMessage> LoginUserAsync(string userName, string password)
    {
        var uri = this.BuildUri("/api/UserToken");
        var request = new UserTokenLoginRequest() { UserName = userName, Password = password };
        var response = await this.httpClient.PostAsJsonAsync(uri, request);
        return response;
    }

    public async Task<HttpResponseMessage> RefreshUserTokenAsync()
    {
        var uri = this.BuildUri("/api/UserToken");
        var response = await this.httpClient.PutAsync(uri, null);
        return response;
    }

    public async Task<HttpResponseMessage> LogoutUserAsync()
    {
        var uri = this.BuildUri("/api/UserToken");
        var response = await this.httpClient.DeleteAsync(uri);
        return response;
    }
}
