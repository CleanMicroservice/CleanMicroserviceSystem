using System.ComponentModel.DataAnnotations;

namespace CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Roles;
public class RoleCreateRequest
{
    [Required(ErrorMessage = "Role name is required")]
    public string RoleName { get; set; }
}
