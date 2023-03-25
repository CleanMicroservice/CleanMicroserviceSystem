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

    public bool? Enabled { get; set; }

    [DataType(DataType.Password)]
    public string? CurrentPassword { get; set; }

    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword), ErrorMessage = "Confirm password does not match with Password.")]
    public string? ConfirmPassword { get; set; }
}
