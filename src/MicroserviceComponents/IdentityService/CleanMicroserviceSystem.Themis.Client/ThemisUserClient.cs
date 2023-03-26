using System.Net.Http.Json;
using CleanMicroserviceSystem.DataStructure;
using CleanMicroserviceSystem.Oceanus.Client.Abstraction;
using CleanMicroserviceSystem.Themis.Contract.Claims;
using CleanMicroserviceSystem.Themis.Contract.Roles;
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

    public async Task<CommonResult?> UpdateCurrentUserAsync(UserUpdateRequest request)
    {
        var uri = this.BuildUri("/api/User");
        var response = await this.httpClient.PutAsJsonAsync(uri, request);
        return await this.GetCommonResult(response);
    }

    public async Task<CommonResult<UserInformationResponse>?> RegisterUserAsync(UserRegisterRequest request)
    {
        var uri = this.BuildUri("/api/User");
        var response = await this.httpClient.PostAsJsonAsync(uri, request);
        return await this.GetCommonResult<UserInformationResponse>(response);
    }

    public async Task<UserInformationResponse?> GetUserAsync(int id)
    {
        var uri = this.BuildUri($"/api/User/{id}");
        var user = await this.httpClient.GetFromJsonAsync<UserInformationResponse>(uri);
        return user;
    }

    public async Task<PaginatedEnumerable<UserInformationResponse>?> SearchUsersAsync(UserSearchRequest request)
    {
        var queryString = new QueryString();
        if (request.Id.HasValue) queryString += QueryString.Create(nameof(request.Id), request.Id.ToString());
        if (request.Count.HasValue) queryString += QueryString.Create(nameof(request.Count), request.Count.ToString());
        if (request.Start.HasValue) queryString += QueryString.Create(nameof(request.Start), request.Start.ToString());
        if (!string.IsNullOrEmpty(request.PhoneNumber)) queryString += QueryString.Create(nameof(request.PhoneNumber), request.PhoneNumber.ToString());
        if (!string.IsNullOrEmpty(request.Email)) queryString += QueryString.Create(nameof(request.Email), request.Email.ToString());
        if (!string.IsNullOrEmpty(request.UserName)) queryString += QueryString.Create(nameof(request.UserName), request.UserName.ToString());
        var uri = this.BuildUri($"/api/User/Search{queryString.ToUriComponent()}");
        var user = await this.httpClient.GetFromJsonAsync<PaginatedEnumerable<UserInformationResponse>>(uri);
        return user;
    }

    public async Task<CommonResult?> UpdateUserAsync(int id, UserUpdateRequest request)
    {
        var uri = this.BuildUri($"/api/User/{id}");
        var response = await this.httpClient.PutAsJsonAsync(uri, request);
        return await this.GetCommonResult(response);
    }

    public async Task<CommonResult?> DeleteUserAsync(int id)
    {
        var uri = this.BuildUri($"/api/User/{id}");
        var commonResult = await this.httpClient.DeleteFromJsonAsync<CommonResult>(uri);
        return commonResult;
    }

    public async Task<IEnumerable<ClaimInformationResponse>?> GetUserClaimsAsync(int id)
    {
        var uri = this.BuildUri($"/api/User/{id}/Claims");
        var claims = await this.httpClient.GetFromJsonAsync<IEnumerable<ClaimInformationResponse>>(uri);
        return claims;
    }

    public async Task<CommonResult?> AddUserClaimsAsync(int id, IEnumerable<ClaimsUpdateRequest> requests)
    {
        var uri = this.BuildUri($"/api/User/{id}/Claims");
        var response = await this.httpClient.PostAsJsonAsync(uri, requests);
        return await this.GetCommonResult(response);
    }

    public async Task<CommonResult?> UpdateUserClaimsAsync(int id, IEnumerable<ClaimsUpdateRequest> requests)
    {
        var uri = this.BuildUri($"/api/User/{id}/Claims");
        var response = await this.httpClient.PutAsJsonAsync(uri, requests);
        return await this.GetCommonResult(response);
    }

    public async Task<CommonResult?> DeleteUserClaimsAsync(int id, IEnumerable<ClaimsUpdateRequest> requests)
    {
        var uri = this.BuildUri($"/api/User/{id}/Claims");
        var response = await this.httpClient.DeleteAsJsonAsync(uri, requests);
        return await this.GetCommonResult(response);
    }

    public async Task<IEnumerable<RoleInformationResponse>?> GetUserRolesAsync(int id)
    {
        var uri = this.BuildUri($"/api/User/{id}/Roles");
        var roles = await this.httpClient.GetFromJsonAsync<IEnumerable<RoleInformationResponse>>(uri);
        return roles;
    }

    public async Task<CommonResult?> AddUserRolesAsync(int id, IEnumerable<string> requests)
    {
        var uri = this.BuildUri($"/api/User/{id}/Roles");
        var response = await this.httpClient.PostAsJsonAsync(uri, requests);
        return await this.GetCommonResult(response);
    }

    public async Task<CommonResult?> UpdateUserRolesAsync(int id, IEnumerable<string> requests)
    {
        var uri = this.BuildUri($"/api/User/{id}/Roles");
        var response = await this.httpClient.PutAsJsonAsync(uri, requests);
        return await this.GetCommonResult(response);
    }

    public async Task<CommonResult?> DeleteUserRolesAsync(int id, IEnumerable<string> requests)
    {
        var uri = this.BuildUri($"/api/User/{id}/Roles");
        var response = await this.httpClient.DeleteAsJsonAsync(uri, requests);
        return await this.GetCommonResult(response);
    }
}
