using CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Repository;
using CleanMicroserviceSystem.Themis.Application.Repository;
using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;
using CleanMicroserviceSystem.Themis.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Themis.Infrastructure.Repository
{
    public class ClientClaimRepository : RepositoryBase<ClientClaim>, IClientClaimRepository
    {
        public ClientClaimRepository(
            ILogger<ClientClaimRepository> logger,
            ConfigurationDbContext dbContext)
            : base(logger, dbContext)
        {
        }
    }
}
