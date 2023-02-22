using System.ComponentModel.DataAnnotations;

namespace CleanMicroserviceSystem.Themis.Domain.DTOs.Roles;

public class RoleUpdateRequest
{
    public string? RoleName { get; set; }
}
