using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Repository;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;
using CleanMicroserviceSystem.Themis.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Repository
{
    public class ClientApiScopeMapRepository : RepositoryBase<ClientApiScopeMap>, IClientApiScopeMapRepository
    {
        public ClientApiScopeMapRepository(
            ILogger<ClientApiScopeMapRepository> logger,
            ConfigurationDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public async Task<IEnumerable<ClientApiScopeMap>> GetClientApiResourceScopeMaps(int clientId, int resourceId)
        {
            return this.AsQueryable().Where(x => x.ClientID == clientId && x.ApiScope.ApiResourceID == resourceId);
        }

        public async Task<IEnumerable<ClientApiScopeMap>> GetClientApiScopeMaps(int clientId)
        {
            return this.AsQueryable().Where(x => x.ClientID == clientId);
        }
    }
}
