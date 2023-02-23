using System.ComponentModel.DataAnnotations;

namespace CleanMicroserviceSystem.Themis.Contract.Clients
{
    public class ClientTokenLoginRequest
    {
        [Required(ErrorMessage = "Client name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Secret is required")]
        public string Secret { get; set; }
    }
}
