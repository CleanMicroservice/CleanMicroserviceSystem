﻿using System.Net.Http.Json;
using CleanMicroserviceSystem.DataStructure;
using CleanMicroserviceSystem.Oceanus.Client.Abstraction;
using CleanMicroserviceSystem.Themis.Contract.Claims;
using CleanMicroserviceSystem.Themis.Contract.Roles;
using CleanMicroserviceSystem.Themis.Contract.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanMicroserviceSystem.Themis.Client;

public class ThemisRoleClient : OceanusServiceClientBase
{
    public ThemisRoleClient(
        ILogger<ThemisRoleClient> logger,
        IHttpClientFactory httpClientFactory,
        IOptionsSnapshot<ThemisClientConfiguration> options) :
        base(logger, httpClientFactory, options.Value.GatewayClientName, ThemisClientContract.ThemisUriPrefix)
    {
    }

    public async Task<CommonResult<RoleInformationResponse>?> CreateRoleAsync(RoleCreateRequest request)
    {
        var uri = this.BuildUri("/api/Role");
        var response = await this.httpClient.PostAsJsonAsync(uri, request);
        return await this.GetCommonResult<RoleInformationResponse>(response);
    }

    public async Task<IEnumerable<RoleInformationResponse>?> GetRolesAsync()
    {
        var uri = this.BuildUri($"/api/Role/Search");
        var roles = await this.httpClient.GetFromJsonAsync<PaginatedEnumerable<RoleInformationResponse>?>(uri);
        return roles?.Values;
    }

    public async Task<RoleInformationResponse?> GetRoleAsync(int id)
    {
        var uri = this.BuildUri($"/api/Role/{id}");
        var role = await this.httpClient.GetFromJsonAsync<RoleInformationResponse>(uri);
        return role;
    }

    public async Task<PaginatedEnumerable<RoleInformationResponse>?> SearchRolesAsync(RoleSearchRequest request)
    {
        var queryString = new QueryString();
        if (request.Id.HasValue) queryString += QueryString.Create(nameof(request.Id), request.Id.ToString());
        if (request.Count.HasValue) queryString += QueryString.Create(nameof(request.Count), request.Count.ToString());
        if (request.Start.HasValue) queryString += QueryString.Create(nameof(request.Start), request.Start.ToString());
        if (!string.IsNullOrEmpty(request.RoleName)) queryString += QueryString.Create(nameof(request.RoleName), request.RoleName.ToString());
        var uri = this.BuildUri($"/api/Role/Search{queryString.ToUriComponent()}");
        var role = await this.httpClient.GetFromJsonAsync<PaginatedEnumerable<RoleInformationResponse>>(uri);
        return role;
    }

    public async Task<CommonResult?> UpdateRoleAsync(int id, RoleUpdateRequest request)
    {
        var uri = this.BuildUri($"/api/Role/{id}");
        var response = await this.httpClient.PutAsJsonAsync(uri, request);
        return await this.GetCommonResult(response);
    }

    public async Task<CommonResult?> DeleteRoleAsync(int id)
    {
        var uri = this.BuildUri($"/api/Role/{id}");
        var response = await this.httpClient.DeleteAsync(uri);
        return await this.GetCommonResult(response);
    }

    public async Task<PaginatedEnumerable<ClaimInformationResponse>?> SearchClaimsAsync(ClaimSearchRequest request)
    {
        var queryString = new QueryString();
        if (request.IdentityId.HasValue) queryString += QueryString.Create(nameof(request.IdentityId), request.IdentityId.ToString());
        if (request.Count.HasValue) queryString += QueryString.Create(nameof(request.Count), request.Count.ToString());
        if (request.Start.HasValue) queryString += QueryString.Create(nameof(request.Start), request.Start.ToString());
        if (!string.IsNullOrEmpty(request.Type)) queryString += QueryString.Create(nameof(request.Type), request.Type.ToString());
        if (!string.IsNullOrEmpty(request.Value)) queryString += QueryString.Create(nameof(request.Value), request.Value.ToString());
        var uri = this.BuildUri($"/api/Role/Claims{queryString.ToUriComponent()}");
        var claims = await this.httpClient.GetFromJsonAsync<PaginatedEnumerable<ClaimInformationResponse>>(uri);
        return claims;
    }

    public async Task<IEnumerable<ClaimInformationResponse>?> GetRoleClaimsAsync(int id)
    {
        var uri = this.BuildUri($"/api/Role/{id}/Claims");
        var claims = await this.httpClient.GetFromJsonAsync<IEnumerable<ClaimInformationResponse>>(uri);
        return claims;
    }

    public async Task<CommonResult?> AddRoleClaimsAsync(int id, IEnumerable<ClaimUpdateRequest> requests)
    {
        var uri = this.BuildUri($"/api/Role/{id}/Claims");
        var response = await this.httpClient.PostAsJsonAsync(uri, requests);
        return await this.GetCommonResult(response);
    }

    public async Task<CommonResult?> UpdateRoleClaimsAsync(int id, IEnumerable<ClaimUpdateRequest> requests)
    {
        var uri = this.BuildUri($"/api/Role/{id}/Claims");
        var response = await this.httpClient.PutAsJsonAsync(uri, requests);
        return await this.GetCommonResult(response);
    }

    public async Task<CommonResult?> DeleteRoleClaimsAsync(int id, IEnumerable<ClaimUpdateRequest> requests)
    {
        var uri = this.BuildUri($"/api/Role/{id}/Claims");
        var response = await this.httpClient.DeleteAsJsonAsync(uri, requests);
        return await this.GetCommonResult(response);
    }

    public async Task<PaginatedEnumerable<UserInformationResponse>?> GetRoleUsersAsync(int id)
    {
        var uri = this.BuildUri($"/api/Role/{id}/Users");
        var roles = await this.httpClient.GetFromJsonAsync<PaginatedEnumerable<UserInformationResponse>>(uri);
        return roles;
    }

    public async Task<CommonResult?> AddRoleUsersAsync(int id, IEnumerable<int> requests)
    {
        var uri = this.BuildUri($"/api/Role/{id}/Users");
        var response = await this.httpClient.PostAsJsonAsync(uri, requests);
        return await this.GetCommonResult(response);
    }
    public async Task<CommonResult?> UpdateRoleUsersAsync(int id, IEnumerable<int> requests)
    {
        var uri = this.BuildUri($"/api/Role/{id}/Users");
        var response = await this.httpClient.PutAsJsonAsync(uri, requests);
        return await this.GetCommonResult(response);
    }


    public async Task<CommonResult?> DeleteRoleUsersAsync(int id, IEnumerable<int> requests)
    {
        var uri = this.BuildUri($"/api/Role/{id}/Users");
        var response = await this.httpClient.DeleteAsJsonAsync(uri, requests);
        return await this.GetCommonResult(response);
    }
}
