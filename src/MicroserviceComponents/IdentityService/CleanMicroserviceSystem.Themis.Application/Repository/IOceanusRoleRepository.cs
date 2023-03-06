using CleanMicroserviceSystem.DataStructure;
using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Repository;
using CleanMicroserviceSystem.Themis.Domain.Entities.Identity;

namespace CleanMicroserviceSystem.Themis.Application.Repository;

public interface IOceanusRoleRepository : IRepositoryBase<OceanusRole>
{
    Task<PaginatedEnumerable<OceanusRole>> SearchAsync(
        int? id,
        string? roleName,
        int? start,
        int? count);

    Task<PaginatedEnumerable<OceanusUser>> SearchUsersAsync(
        IEnumerable<int> roleIds,
        int? id,
        string? userName,
        string? email,
        string? phoneNumber,
        int? start,
        int? count);
}
