using System.ComponentModel.DataAnnotations;

namespace CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Clients
{
    public class ApiScopeCreateRequest
    {
        [Required(ErrorMessage = "Api scope name is required")]
        public string Name { get; set; }

        public bool Enabled { get; set; }

        public string? Description { get; set; }
    }
}
