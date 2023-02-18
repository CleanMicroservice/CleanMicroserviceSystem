using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Themis.Application.DataTransferObjects.ApiResources;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Application.Services;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Services
{
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
            int? id, string? name, bool? enabled, int start, int count)
        {
            return await this.apiResourceRepository.SearchAsync(id, name, enabled, start, count);
        }


        public async Task<ApiResourceResult> CreateAsync(ApiResource resource)
        {
            resource = await this.apiResourceRepository.AddAsync(resource);
            await this.apiResourceRepository.SaveChangesAsync();
            return new ApiResourceResult() { ApiResource = resource };
        }

        public async Task<ApiResourceResult> UpdateAsync(ApiResource resource)
        {
            resource = await this.apiResourceRepository.UpdateAsync(resource);
            await this.apiResourceRepository.SaveChangesAsync();
            return new ApiResourceResult() { ApiResource = resource };
        }

        public async Task<ApiResourceResult> DeleteAsync(ApiResource resource)
        {
            await this.apiResourceRepository.RemoveAsync(resource);
            await this.apiResourceRepository.SaveChangesAsync();
            return new ApiResourceResult() { ApiResource = resource };
        }

        public async Task<ApiResource?> FindByIdAsync(int resourceId)
        {
            return await this.apiResourceRepository.FindAsync(resourceId);
        }
    }
}
