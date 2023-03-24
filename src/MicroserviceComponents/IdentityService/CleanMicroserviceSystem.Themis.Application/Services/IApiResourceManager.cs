using CleanMicroserviceSystem.DataStructure;
using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Models;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;

namespace CleanMicroserviceSystem.Themis.Application.Services;

public interface IApiResourceManager
{
    Task<PaginatedEnumerable<ApiResource>> SearchAsync(
        int? id, string? name, bool? enabled, int? start, int? count);

    Task<ApiResource?> FindByIdAsync(int resourceId);

    Task<CommonResult<ApiResource>> CreateAsync(ApiResource resource);

    Task<CommonResult> UpdateAsync(ApiResource resource);

    Task<CommonResult> DeleteAsync(ApiResource resource);
}
