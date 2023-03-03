using System.Data;
using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Repository;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Domain.Entities.Identity;
using CleanMicroserviceSystem.Themis.Infrastructure.Persistence;
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
            users = users.Where(user => EF.Functions.Like(user.UserName, $"%{userName}%"));
        if (!string.IsNullOrEmpty(email))
            users = users.Where(user => EF.Functions.Like(user.Email, $"%{email}%"));
        if (!string.IsNullOrEmpty(phoneNumber))
            users = users.Where(user => EF.Functions.Like(user.PhoneNumber, $"%{phoneNumber}%"));

        var originCounts = await users.CountAsync();
        users = users.OrderBy(user => user.Id);
        if (start.HasValue)
            users = users.Skip(start.Value);
        if (count.HasValue)
            users = users.Take(count.Value);
        return new PaginatedEnumerable<OceanusUser>(users.ToArray(), start, count, originCounts);
    }
}
