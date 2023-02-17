using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Themis.Application.DataTransferObjects.ApiResources;
using CleanMicroserviceSystem.Themis.Application.DataTransferObjects.ApiScopes;
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
        private readonly IApiScopeRepository apiScopeRepository;

        public ApiResourceManager(
            ILogger<ApiResourceManager> logger,
            IApiResourceRepository apiResourceRepository,
            IApiScopeRepository apiScopeRepository)
        {
            this.logger = logger;
            this.apiResourceRepository = apiResourceRepository;
            this.apiScopeRepository = apiScopeRepository;
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

        public async Task<ApiResource?> FindResourceByIdAsync(int resourceId)
        {
            return await this.apiResourceRepository.FindAsync(resourceId);
        }

        public async Task<ApiScope?> FindScopeByIdAsync(int scopeId)
        {
            return await this.apiScopeRepository.FindAsync(scopeId);
        }

        public async Task<IEnumerable<ApiScope>?> GetResourceScopesAsync(int resourceId)
        {
            var resource = await this.apiResourceRepository.FindAsync(resourceId);
            if (resource is null) return null;

            var scopes = await this.apiScopeRepository.GetResourceScopes(resourceId);
            return scopes;
        }

        public async Task<ApiScopeResult> CreateScopeAsync(ApiScope scope)
        {
            scope = await this.apiScopeRepository.AddAsync(scope);
            await this.apiResourceRepository.SaveChangesAsync();
            return new ApiScopeResult() { ApiScope = scope };
        }

        public async Task<ApiScopeResult?> DeleteScopeAsync(ApiScope scope)
        {
            scope = await this.apiScopeRepository.RemoveAsync(scope);
            await this.apiResourceRepository.SaveChangesAsync();
            return new ApiScopeResult() { ApiScope = scope };
        }
    }
}
