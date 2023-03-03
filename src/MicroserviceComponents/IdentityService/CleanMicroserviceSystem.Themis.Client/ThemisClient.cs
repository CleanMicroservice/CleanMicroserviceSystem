using System.Net.Http.Json;
using CleanMicroserviceSystem.Gateway.Contract;
using CleanMicroserviceSystem.Oceanus.Client.Abstraction;
using CleanMicroserviceSystem.Themis.Contract.Users;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanMicroserviceSystem.Themis.Client;

public class ThemisClient : OceanusServiceClientBase
{
    public ThemisClient(
        ILogger<OceanusServiceClientBase> logger,
        IHttpClientFactory httpClientFactory,
        IOptionsSnapshot<ThemisClientConfiguration> options) :
        base(logger, httpClientFactory, options.Value.GatewayClientName, $"{GatewayContract.GatewayUriPrefix}/Themis")
    {
    }

    public async Task<HttpResponseMessage> LoginUserAsync(string userName, string password)
    {
        var uri = this.BuildUri("/api/UserToken");
        var request = new UserTokenLoginRequest() { UserName = userName, Password = password };
        var response = await this.httpClient.PostAsJsonAsync(uri, request);
        return response;
    }

    public async Task<UserInformationResponse?> GetCurrentUserAsync()
    {
        var uri = this.BuildUri("/api/User");
        var user = await this.httpClient.GetFromJsonAsync<UserInformationResponse>(uri);
        return user;
    }
}
