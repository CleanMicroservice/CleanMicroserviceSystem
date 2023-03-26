using CleanMicroserviceSystem.DataStructure;
using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Repository;
using CleanMicroserviceSystem.Themis.Domain.Entities.Identity;

namespace CleanMicroserviceSystem.Themis.Application.Repository;

public interface IOceanusUserRepository : IRepositoryBase<OceanusUser>
{
    Task<PaginatedEnumerable<OceanusRole>> GetRolesAsync(int userId, int? start, int? count);

    Task<PaginatedEnumerable<OceanusUser>> Search(
        int? id, string? userName, string? email, string? phoneNumber, int? start, int? count);
    
    Task<PaginatedEnumerable<OceanusRole>> SearchRolesAsync(
        int userId, int? id, string? roleName, int? start, int? count);
}
