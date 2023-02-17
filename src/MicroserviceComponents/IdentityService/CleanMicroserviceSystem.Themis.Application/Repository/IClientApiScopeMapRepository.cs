using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Repository;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;

namespace CleanMicroserviceSystem.Themis.Application.Repository
{
    public interface IClientApiScopeMapRepository : IRepositoryBase<ClientApiScopeMap>
    {
        Task<IEnumerable<ClientApiScopeMap>> GetClientApiResourceScopeMaps(int clientId, int resourceId);

        Task<IEnumerable<ClientApiScopeMap>> GetClientApiScopeMaps(int clientId);
    }
}
