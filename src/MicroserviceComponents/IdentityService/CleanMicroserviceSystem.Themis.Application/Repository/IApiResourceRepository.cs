using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Repository;
using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;

namespace CleanMicroserviceSystem.Themis.Application.Repository;

public interface IApiResourceRepository : IRepositoryBase<ApiResource>
{
    Task<PaginatedEnumerable<ApiResource>> SearchAsync(
        int? id,
        string? name,
        bool? enabled,
        int? start,
        int? count);
}
