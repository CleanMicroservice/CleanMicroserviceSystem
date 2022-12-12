using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Repository;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Repository;

public class OceanusRoleRepository : RepositoryBase<OceanusRole>, IOceanusRoleRepository
{
    public OceanusRoleRepository(
        ILogger<OceanusRoleRepository> logger,
        DbContext dbContext)
        : base(logger, dbContext)
    {
    }

    public async Task<IEnumerable<OceanusRole>> Search(int? id, string? roleName, int start, int count)
    {
        var roles = this.AsQueryable();
        if (id.HasValue)
            roles = roles.Where(role => role.Id == id);
        if (!string.IsNullOrEmpty(roleName))
            roles = roles.Where(role => EF.Functions.Like(role.Name, $"%{roleName}%"));
        roles = roles.Skip(start).Take(count);
        return roles.AsEnumerable();
    }
}
