using CleanMicroserviceSystem.Common.Domain.Entities;
using CleanMicroserviceSystem.Themis.Domain.Identity;

namespace CleanMicroserviceSystem.Themis.Application.Repository;

public interface IOceanusRoleRepository
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
