using System.Security.Claims;
using CleanMicroserviceSystem.DataStructure;
using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Repository;
using CleanMicroserviceSystem.Themis.Domain.Entities.Identity;

namespace CleanMicroserviceSystem.Themis.Application.Repository;

public interface IOceanusRoleRepository : IRepositoryBase<OceanusRole>
{
    Task<PaginatedEnumerable<Claim>> SearchClaims(
        int? roleId, string? type, string? value, int? start, int? count);

    Task<PaginatedEnumerable<OceanusRole>> SearchAsync(
        int? id,
        string? roleName,
        int? start,
        int? count);

    Task<PaginatedEnumerable<OceanusUser>> SearchUsersAsync(
        int roleId,
        int? id,
        string? userName,
        string? email,
        string? phoneNumber,
        int? start,
        int? count);

    Task<PaginatedEnumerable<OceanusUser>> GetUsersAsync(
        int roleId,
        int? start,
        int? count);
}
