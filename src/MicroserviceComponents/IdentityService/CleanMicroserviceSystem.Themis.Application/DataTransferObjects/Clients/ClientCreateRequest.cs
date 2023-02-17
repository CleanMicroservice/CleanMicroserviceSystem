using System.ComponentModel.DataAnnotations;

namespace CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Clients
{
    public class ClientCreateRequest
    {
        [Required(ErrorMessage = "Client name is required")]
        public string Name { get; set; }

        public bool Enabled { get; set; }

        public string? Description { get; set; }

        public string? Secret { get; set; }
    }
}
