using System.ComponentModel.DataAnnotations;
using CleanMicroserviceSystem.Oceanus.Contract.Abstraction;

namespace CleanMicroserviceSystem.Themis.Contract.ApiResources;

public class ApiResourceCreateRequest : ContractBase
{
    [Required(ErrorMessage = "Api resource name is required")]
    public string Name { get; set; } = default!;

    public bool Enabled { get; set; }

    public string? Description { get; set; }
}
