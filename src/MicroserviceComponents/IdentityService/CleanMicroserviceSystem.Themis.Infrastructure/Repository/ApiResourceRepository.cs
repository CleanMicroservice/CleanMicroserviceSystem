using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Repository;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;
using CleanMicroserviceSystem.Themis.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Repository
{
    public class ApiResourceRepository : RepositoryBase<ApiResource>, IApiResourceRepository
    {
        public ApiResourceRepository(
            ILogger<ApiResourceRepository> logger,
            ConfigurationDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public async Task<PaginatedEnumerable<ApiResource>> SearchAsync(
            int? id,
            string? name,
            bool? enabled,
            int start,
            int count)
        {
            var resources = this.AsQueryable();
            if (id.HasValue)
                resources = resources.Where(resource => resource.Id == id);
            if (!string.IsNullOrEmpty(name))
                resources = resources.Where(resource => EF.Functions.Like(resource.Name, $"%{name}%"));
            if (enabled.HasValue)
                resources = resources.Where(resource => resource.Enabled == enabled.Value);
            var originCounts = await resources.CountAsync();
            resources = resources.Skip(start).Take(count);
            return new PaginatedEnumerable<ApiResource>(resources.ToArray(), start, count, originCounts);
        }
    }
}
