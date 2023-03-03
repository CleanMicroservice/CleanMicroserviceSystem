using CleanMicroserviceSystem.Oceanus.Contract.Abstraction;

namespace CleanMicroserviceSystem.Themis.Contract.Roles;

public class RoleSearchRequest : ContractBase
{
    public int? Id { get; set; } = default;

    public string? RoleName { get; set; } = default;

    public int? Start { get; set; }

    public int? Count { get; set; }
}
