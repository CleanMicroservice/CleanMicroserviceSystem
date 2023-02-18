using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Themis.Application.DataTransferObjects.ApiResources;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;

namespace CleanMicroserviceSystem.Themis.Application.Services
{
    public interface IApiResourceManager
    {
        Task<PaginatedEnumerable<ApiResource>> SearchAsync(
            int? id, string? name, bool? enabled, int start, int count);

        Task<ApiResource?> FindByIdAsync(int resourceId);

        Task<ApiResourceResult> CreateAsync(ApiResource resource);

        Task<ApiResourceResult> UpdateAsync(ApiResource resource);

        Task<ApiResourceResult> DeleteAsync(ApiResource resource);
    }
}
