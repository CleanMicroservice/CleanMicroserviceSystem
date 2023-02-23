using System.ComponentModel.DataAnnotations;

namespace CleanMicroserviceSystem.Themis.Contract.Roles;
public class RoleCreateRequest
{
    [Required(ErrorMessage = "Role name is required")]
    public string RoleName { get; set; }
}
