using System.Data;
using System.Security.Claims;
using CleanMicroserviceSystem.DataStructure;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Repository;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Domain.Entities.Identity;
using CleanMicroserviceSystem.Themis.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Repository;

public class OceanusUserRepository : RepositoryBase<OceanusUser>, IOceanusUserRepository
{
    public OceanusUserRepository(
        ILogger<OceanusUserRepository> logger,
        IdentityDbContext dbContext)
        : base(logger, dbContext)
    {
    }

    public async Task<PaginatedEnumerable<Claim>> SearchClaims(
        int? userId,
        string? type,
        string? value,
        int? start,
        int? count)
    {
        var userClaims = this.dbContext.Set<IdentityUserClaim<int>>().AsQueryable();

        if (userId.HasValue)
            userClaims = userClaims.Where(userClaim => userClaim.UserId == userId);
        if (!string.IsNullOrEmpty(type))
            userClaims = userClaims.Where(userClaim => EF.Functions.Like(userClaim.ClaimType!, $"%{type}%"));
        if (!string.IsNullOrEmpty(value))
            userClaims = userClaims.Where(userClaim => EF.Functions.Like(userClaim.ClaimValue!, $"%{value}%"));

        var claims = userClaims.Select(userClaim => new { userClaim.ClaimType, userClaim.ClaimValue }).Distinct();
        var originCounts = await claims.CountAsync();
        claims = claims.OrderBy(claim => claim.ClaimType).ThenBy(claim => claim.ClaimValue);
        if (start.HasValue)
            claims = claims.Skip(start.Value);
        if (count.HasValue)
            claims = claims.Take(count.Value);
        var result = claims.Select(claim => new Claim(claim.ClaimType!, claim.ClaimValue!));
        return new PaginatedEnumerable<Claim>(result, start, count, originCounts);
    }

    public async Task<PaginatedEnumerable<OceanusUser>> Search(
        int? id,
        string? userName,
        string? email,
        string? phoneNumber,
        int? start,
        int? count)
    {
        var users = this.AsQueryable();
        if (id.HasValue)
            users = users.Where(user => user.Id == id);
        if (!string.IsNullOrEmpty(userName))
            users = users.Where(user => EF.Functions.Like(user.UserName!, $"%{userName}%"));
        if (!string.IsNullOrEmpty(email))
            users = users.Where(user => EF.Functions.Like(user.Email!, $"%{email}%"));
        if (!string.IsNullOrEmpty(phoneNumber))
            users = users.Where(user => EF.Functions.Like(user.PhoneNumber!, $"%{phoneNumber}%"));

        var originCounts = await users.CountAsync();
        users = users.OrderBy(user => user.Id);
        if (start.HasValue)
            users = users.Skip(start.Value);
        if (count.HasValue)
            users = users.Take(count.Value);
        return new PaginatedEnumerable<OceanusUser>(users.ToArray(), start, count, originCounts);
    }

    public async Task<PaginatedEnumerable<OceanusRole>> SearchRolesAsync(
        int userId,
        int? id,
        string? roleName,
        int? start,
        int? count)
    {
        var roles = this.AsQueryable()
            .Where(user => userId == user.Id)
            .Join(this.dbContext.Set<IdentityUserRole<int>>(), user => user.Id, map => map.UserId, (user, map) => new { User = user, map.RoleId })
            .Join(this.dbContext.Set<OceanusRole>(), tuple => tuple.RoleId, role => role.Id, (map, role) => role);

        if (id.HasValue)
            roles = roles.Where(role => role.Id == id);
        if (!string.IsNullOrEmpty(roleName))
            roles = roles.Where(role => EF.Functions.Like(role.Name!, $"%{roleName}%"));

        var originCounts = await roles.CountAsync();
        roles = roles.OrderBy(user => user.Id);
        if (start.HasValue)
            roles = roles.Skip(start.Value);
        if (count.HasValue)
            roles = roles.Take(count.Value);
        return new PaginatedEnumerable<OceanusRole>(roles.ToArray(), start, count, originCounts);
    }

    public async Task<PaginatedEnumerable<OceanusRole>> GetRolesAsync(
        int userId,
        int? start,
        int? count)
    {
        var roles = this.AsQueryable()
            .Where(user => userId == user.Id)
            .Join(this.dbContext.Set<IdentityUserRole<int>>(), user => user.Id, map => map.UserId, (user, map) => new { User = user, map.RoleId })
            .Join(this.dbContext.Set<OceanusRole>(), tuple => tuple.RoleId, role => role.Id, (map, role) => role);

        var originCounts = await roles.CountAsync();
        roles = roles.OrderBy(user => user.Id);
        if (start.HasValue)
            roles = roles.Skip(start.Value);
        if (count.HasValue)
            roles = roles.Take(count.Value);
        return new PaginatedEnumerable<OceanusRole>(roles.ToArray(), start, count, originCounts);
    }
}
