using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Repository;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Repository;

public class OceanusUserRepository : RepositoryBase<OceanusUser>, IOceanusUserRepository
{
    public OceanusUserRepository(
        ILogger<OceanusUserRepository> logger,
        DbContext dbContext)
        : base(logger, dbContext)
    {
    }

    public async Task<IEnumerable<OceanusUser>> Search(
        int? id,
        string? userName,
        string? email,
        string? phoneNumber,
        int start,
        int count)
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
        users = users.Skip(start).Take(count);
        return users.AsEnumerable();
    }
}
