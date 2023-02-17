using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Contracts;

namespace CleanMicroserviceSystem.Themis.Domain.Entities.Configuration
{
    public class ClientApiScopeMap : AuditableEntity
    {
        public int ClientID { get; set; }

        public virtual Client Client { get; set; }

        public int ApiScopeID { get; set; }

        public virtual ApiScope ApiScope { get; set; }
    }
}
