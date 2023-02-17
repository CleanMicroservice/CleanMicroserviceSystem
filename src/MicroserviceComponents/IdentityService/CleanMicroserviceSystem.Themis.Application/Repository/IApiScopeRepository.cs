using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Repository;
using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;

namespace CleanMicroserviceSystem.Themis.Application.Repository
{
    public interface IApiScopeRepository : IRepositoryBase<ApiScope>
    {
        Task<PaginatedEnumerable<ApiScope>> SearchAsync(
            int? id,
            string? name,
            bool? enabled,
            int start,
            int count);
    }
}
