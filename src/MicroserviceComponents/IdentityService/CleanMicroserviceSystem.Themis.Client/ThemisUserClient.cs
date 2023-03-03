using System.Net.Http.Json;
using CleanMicroserviceSystem.Oceanus.Client.Abstraction;
using CleanMicroserviceSystem.Themis.Contract.Users;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanMicroserviceSystem.Themis.Client;

public class ThemisUserClient : OceanusServiceClientBase
{
    public ThemisUserClient(
        ILogger<ThemisUserClient> logger,
        IHttpClientFactory httpClientFactory,
        IOptionsSnapshot<ThemisClientConfiguration> options) :
        base(logger, httpClientFactory, options.Value.GatewayClientName, ThemisClientContract.ThemisUriPrefix)
    {
    }

    public async Task<UserInformationResponse?> GetCurrentUserAsync()
    {
        var uri = this.BuildUri("/api/User");
        var user = await this.httpClient.GetFromJsonAsync<UserInformationResponse>(uri);
        return user;
    }
}
