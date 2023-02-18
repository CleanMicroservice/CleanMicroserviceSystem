using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Contracts;

namespace CleanMicroserviceSystem.Themis.Domain.Entities.Configuration
{
    public class Client : AuditableEntity<int>
    {
        [Required]
        public string Name { get; set; }

        [DefaultValue(true)]
        public bool Enabled { get; set; }

        public string? Description { get; set; }

        public string? Secret { get; set; }

        public virtual IEnumerable<ClientClaim>? Claims { get; set; }
    }
}
