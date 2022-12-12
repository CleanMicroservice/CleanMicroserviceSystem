using CleanMicroserviceSystem.Themis.Domain.Identity;

namespace CleanMicroserviceSystem.Themis.Application.Repository;

public interface IOceanusRoleRepository
{
    Task<IEnumerable<OceanusRole>> Search(
        int? id,
        string? roleName,
        int start,
        int count);
}
