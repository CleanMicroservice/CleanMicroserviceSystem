using System.ComponentModel.DataAnnotations;

namespace CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Roles;

public class RoleUpdateRequest
{
    public string? RoleName { get; set; }
}
