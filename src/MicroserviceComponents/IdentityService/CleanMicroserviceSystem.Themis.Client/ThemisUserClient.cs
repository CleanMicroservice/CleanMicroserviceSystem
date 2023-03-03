using System.Net.Http.Json;
using CleanMicroserviceSystem.Oceanus.Client.Abstraction;
using CleanMicroserviceSystem.Themis.Contract.Users;
using Microsoft.AspNetCore.Http;
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

    public async Task<HttpResponseMessage?> UpdateCurrentUserAsync(UserUpdateRequest request)
    {
        var uri = this.BuildUri("/api/User");
        var response = await this.httpClient.PutAsJsonAsync(uri, request);
        return response;
    }

    public async Task<UserInformationResponse?> RegisterUserAsync(UserRegisterRequest request)
    {
        var uri = this.BuildUri("/api/User");
        var response = await this.httpClient.PostAsJsonAsync(uri, request);
        var user = await response.Content.ReadFromJsonAsync<UserInformationResponse>();
        return user;
    }

    public async Task<UserInformationResponse?> GetUserAsync(int id)
    {
        var uri = this.BuildUri($"/api/User/{id}");
        var user = await this.httpClient.GetFromJsonAsync<UserInformationResponse>(uri);
        return user;
    }

    public async Task<UserInformationResponse?> SearchUsersAsync(UserSearchRequest request)
    {
        var queryString = new QueryString();
        if (request.Id.HasValue) queryString.Add(nameof(request.Id), request.Id.ToString());
        if (request.Count.HasValue) queryString.Add(nameof(request.Count), request.Count.ToString());
        if (request.Start.HasValue) queryString.Add(nameof(request.Start), request.Start.ToString());
        if (!string.IsNullOrEmpty(request.PhoneNumber)) queryString.Add(nameof(request.PhoneNumber), request.PhoneNumber.ToString());
        if (!string.IsNullOrEmpty(request.Email)) queryString.Add(nameof(request.Email), request.Email.ToString());
        if (!string.IsNullOrEmpty(request.UserName)) queryString.Add(nameof(request.UserName), request.UserName.ToString());
        var uri = this.BuildUri($"/api/User/Search?{queryString.ToUriComponent()}");
        var user = await this.httpClient.GetFromJsonAsync<UserInformationResponse>(uri);
        return user;
    }

    public async Task<HttpResponseMessage?> UpdateUserAsync(int id, UserUpdateRequest request)
    {
        var uri = this.BuildUri($"/api/User/{id}");
        var response = await this.httpClient.PutAsJsonAsync(uri, request);
        return response;
    }

    public async Task<HttpResponseMessage?> DeleteUserAsync(int id)
    {
        var uri = this.BuildUri($"/api/User/{id}");
        var response = await this.httpClient.DeleteAsync(uri);
        return response;
    }
}
