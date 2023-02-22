namespace CleanMicroserviceSystem.Themis.Domain.DTOs.Roles;

public class RoleSearchRequest
{
    public int? Id { get; set; } = default;

    public string? RoleName { get; set; } = default;

    public int Start { get; set; } = 0;

    public int Count { get; set; } = 10;
}
