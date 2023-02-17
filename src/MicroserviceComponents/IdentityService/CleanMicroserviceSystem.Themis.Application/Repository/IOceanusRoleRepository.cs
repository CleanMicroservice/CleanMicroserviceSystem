using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Repository;
using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Themis.Domain.Entities.Identity;

namespace CleanMicroserviceSystem.Themis.Application.Repository;

public interface IOceanusRoleRepository : IRepositoryBase<OceanusRole>
{
    Task<PaginatedEnumerable<OceanusRole>> Search(
        int? id,
        string? roleName,
        int start,
        int count);

    Task<PaginatedEnumerable<OceanusUser>> SearchUsers(
        IEnumerable<int> roleIds,
        int? id,
        string? userName,
        string? email,
        string? phoneNumber,
        int start,
        int count);
}
