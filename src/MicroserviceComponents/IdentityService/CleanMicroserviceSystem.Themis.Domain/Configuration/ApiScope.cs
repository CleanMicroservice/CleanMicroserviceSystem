using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Contracts;

namespace CleanMicroserviceSystem.Themis.Domain.Configuration
{
    public class ApiScope : AuditableEntity<int>
    {
        [Required]
        public string Name { get; set; }

        [DefaultValue(true)]
        public bool Enabled { get; set; }

        public string? Description { get; set; }

        public int ApiResourceID { get; set; }

        public virtual ApiResource ApiResource { get; set; }

        public virtual IEnumerable<ClientApiScopeMap>? ClientMaps { get; set; }
    }
}
