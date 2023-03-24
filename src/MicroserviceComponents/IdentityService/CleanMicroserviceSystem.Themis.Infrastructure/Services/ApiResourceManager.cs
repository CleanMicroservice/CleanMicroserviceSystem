using CleanMicroserviceSystem.DataStructure;
using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Models;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Application.Services;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Services;

public class ApiResourceManager : IApiResourceManager
{

    private readonly ILogger<ApiResourceManager> logger;
    private readonly IApiResourceRepository apiResourceRepository;

    public ApiResourceManager(
        ILogger<ApiResourceManager> logger,
        IApiResourceRepository apiResourceRepository)
    {
        this.logger = logger;
        this.apiResourceRepository = apiResourceRepository;
    }

    public async Task<PaginatedEnumerable<ApiResource>> SearchAsync(
        int? id, string? name, bool? enabled, int? start, int? count)
    {
        this.logger.LogDebug($"Search api resources: {id}, {name}, {enabled}, {start}, {count}");
        return await this.apiResourceRepository.SearchAsync(id, name, enabled, start, count);
    }

    public async Task<CommonResult<ApiResource>> CreateAsync(ApiResource resource)
    {
        this.logger.LogDebug($"Create api resource: {resource.Name}");
        resource = await this.apiResourceRepository.AddAsync(resource);
        _ = await this.apiResourceRepository.SaveChangesAsync();
        return new CommonResult<ApiResource>() { Entity = resource };
    }

    public async Task<CommonResult> UpdateAsync(ApiResource resource)
    {
        this.logger.LogDebug($"Update api resource: {resource.Id}");
        resource = await this.apiResourceRepository.UpdateAsync(resource);
        _ = await this.apiResourceRepository.SaveChangesAsync();
        return new CommonResult();
    }

    public async Task<CommonResult> DeleteAsync(ApiResource resource)
    {
        this.logger.LogDebug($"Update api resource: {resource.Id}");
        _ = await this.apiResourceRepository.RemoveAsync(resource);
        _ = await this.apiResourceRepository.SaveChangesAsync();
        return new CommonResult();
    }

    public async Task<ApiResource?> FindByIdAsync(int resourceId)
    {
        this.logger.LogDebug($"Find api resource: {resourceId}");
        return await this.apiResourceRepository.FindAsync(resourceId);
    }
}
