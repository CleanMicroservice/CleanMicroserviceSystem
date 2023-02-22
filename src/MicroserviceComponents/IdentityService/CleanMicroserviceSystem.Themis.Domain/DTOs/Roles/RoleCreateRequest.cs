using System.ComponentModel.DataAnnotations;

namespace CleanMicroserviceSystem.Themis.Domain.DTOs.Roles;
public class RoleCreateRequest
{
    [Required(ErrorMessage = "Role name is required")]
    public string RoleName { get; set; }
}
