using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Repository;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Domain.Entities.Identity;
using CleanMicroserviceSystem.Themis.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Repository;

public class OceanusRoleRepository : RepositoryBase<OceanusRole>, IOceanusRoleRepository
{
    public OceanusRoleRepository(
        ILogger<OceanusRoleRepository> logger,
        IdentityDbContext dbContext)
        : base(logger, dbContext)
    {
    }

    public async Task<PaginatedEnumerable<OceanusRole>> SearchAsync(int? id, string? roleName, int start, int count)
    {
        var roles = this.AsQueryable();
        if (id.HasValue)
            roles = roles.Where(role => role.Id == id);
        if (!string.IsNullOrEmpty(roleName))
            roles = roles.Where(role => EF.Functions.Like(role.Name, $"%{roleName}%"));
        var originCounts = await roles.CountAsync();
        roles = roles.Skip(start).Take(count);
        return new PaginatedEnumerable<OceanusRole>(roles.ToArray(), start, count, originCounts);
    }

    public async Task<PaginatedEnumerable<OceanusUser>> SearchUsersAsync(
        IEnumerable<int> roleIds,
        int? id,
        string? userName,
        string? email,
        string? phoneNumber,
        int start,
        int count)
    {
        var users = this.AsQueryable()
            .Where(role => roleIds.Contains(role.Id))
            .Join(this.dbContext.Set<IdentityUserRole<int>>(), role => role.Id, map => map.RoleId, (role, map) => new { Role = role, map.UserId })
            .Join(this.dbContext.Set<OceanusUser>(), tuple => tuple.UserId, user => user.Id, (map, user) => user);

        if (id.HasValue)
            users = users.Where(user => user.Id == id);
        if (!string.IsNullOrEmpty(userName))
            users = users.Where(user => EF.Functions.Like(user.UserName, $"%{userName}%"));
        if (!string.IsNullOrEmpty(email))
            users = users.Where(user => EF.Functions.Like(user.Email, $"%{email}%"));
        if (!string.IsNullOrEmpty(phoneNumber))
            users = users.Where(user => EF.Functions.Like(user.PhoneNumber, $"%{phoneNumber}%"));

        var originCounts = await users.CountAsync();
        users = users.OrderBy(user => user.Id).Skip(start).Take(count);
        return new PaginatedEnumerable<OceanusUser>(users.ToArray(), start, count, originCounts);
    }
}
