using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Themis.Domain.Entities.Identity;

namespace CleanMicroserviceSystem.Themis.Application.Repository;

public interface IOceanusUserRepository
{
    Task<PaginatedEnumerable<OceanusUser>> Search(
        int? id,
        string? userName,
        string? email,
        string? phoneNumber,
        int start,
        int count);
}
