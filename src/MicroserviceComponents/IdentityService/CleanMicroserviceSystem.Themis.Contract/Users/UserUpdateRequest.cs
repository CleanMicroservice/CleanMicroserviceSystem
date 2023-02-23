using System.ComponentModel.DataAnnotations;
using CleanMicroserviceSystem.Oceanus.Contract.Abstraction;

namespace CleanMicroserviceSystem.Themis.Contract.Users;
public class UserUpdateRequest : ContractBase
{
    public string? UserName { get; set; }

    [EmailAddress(ErrorMessage = "User Email should match Email format")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "User Phone number should match phone number format")]
    public string? PhoneNumber { get; set; }
}
