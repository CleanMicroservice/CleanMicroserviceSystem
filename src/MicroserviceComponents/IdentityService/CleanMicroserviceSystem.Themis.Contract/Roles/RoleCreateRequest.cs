using System.ComponentModel.DataAnnotations;
using CleanMicroserviceSystem.Oceanus.Contract.Abstraction;

namespace CleanMicroserviceSystem.Themis.Contract.Roles;
public class RoleCreateRequest : ContractBase
{
    [Required(ErrorMessage = "Role name is required")]
    public string RoleName { get; set; }
}
