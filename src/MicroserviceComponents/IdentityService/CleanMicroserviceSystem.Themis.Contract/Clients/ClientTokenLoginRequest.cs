using System.ComponentModel.DataAnnotations;
using CleanMicroserviceSystem.Oceanus.Contract.Abstraction;

namespace CleanMicroserviceSystem.Themis.Contract.Clients;

public class ClientTokenLoginRequest : ContractBase
{
    [Required(ErrorMessage = "Client name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Secret is required")]
    public string Secret { get; set; }
}
