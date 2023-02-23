using System.ComponentModel.DataAnnotations;

namespace CleanMicroserviceSystem.Themis.Contract.Roles;

public class RoleUpdateRequest
{
    public string? RoleName { get; set; }
}
