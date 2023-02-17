using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Repository;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;
using CleanMicroserviceSystem.Themis.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Repository
{
    public class ApiScopeRepository : RepositoryBase<ApiScope>, IApiScopeRepository
    {
        public ApiScopeRepository(
            ILogger<ApiScopeRepository> logger,
            ConfigurationDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public async Task<IEnumerable<ApiScope>> GetResourceScopes(int resourceId)
        {
            return this.AsQueryable().Where(x => x.ApiResourceID == resourceId);
        }

        public async Task<PaginatedEnumerable<ApiScope>> SearchAsync(
            int? id,
            string? name,
            bool? enabled,
            int start,
            int count)
        {
            var scopes = this.AsQueryable();
            if (id.HasValue)
                scopes = scopes.Where(scope => scope.ID == id);
            if (!string.IsNullOrEmpty(name))
                scopes = scopes.Where(scope => EF.Functions.Like(scope.Name, $"%{name}%"));
            if (enabled.HasValue)
                scopes = scopes.Where(scope => scope.Enabled == enabled.Value);
            var originCounts = await scopes.CountAsync();
            scopes = scopes.Skip(start).Take(count);
            return new PaginatedEnumerable<ApiScope>(scopes.ToArray(), start, count, originCounts);
        }
    }
}
