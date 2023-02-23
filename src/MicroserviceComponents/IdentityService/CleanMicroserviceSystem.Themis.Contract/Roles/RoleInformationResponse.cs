using CleanMicroserviceSystem.Oceanus.Contract.Abstraction;

namespace CleanMicroserviceSystem.Themis.Contract.Roles;

public class RoleInformationResponse : ContractBase
{
    public int Id { get; set; }

    public string? RoleName { get; set; }
}
