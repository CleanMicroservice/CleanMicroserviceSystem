using System.ComponentModel.DataAnnotations;
using CleanMicroserviceSystem.Oceanus.Contract.Abstraction;

namespace CleanMicroserviceSystem.Themis.Contract.Clients;

public class ClientCreateRequest : ContractBase
{
    [Required(ErrorMessage = "Client name is required")]
    public string Name { get; set; }

    public bool Enabled { get; set; }

    public string? Description { get; set; }

    public string? Secret { get; set; }
}
