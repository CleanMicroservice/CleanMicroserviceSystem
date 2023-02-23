using CleanMicroserviceSystem.Oceanus.Contract.Abstraction;

namespace CleanMicroserviceSystem.Themis.Contract.Users;

public class UserInformationResponse : ContractBase
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }
}
