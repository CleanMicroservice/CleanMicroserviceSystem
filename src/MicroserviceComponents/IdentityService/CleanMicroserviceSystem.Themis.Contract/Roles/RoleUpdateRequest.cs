using CleanMicroserviceSystem.Oceanus.Contract.Abstraction;

namespace CleanMicroserviceSystem.Themis.Contract.Roles;

public class RoleUpdateRequest : ContractBase
{
    public string? RoleName { get; set; }
}
