using System.ComponentModel.DataAnnotations;

namespace CleanMicroserviceSystem.Themis.Domain.DTOs.Users;
public class UserRegisterRequest
{
    [Required(ErrorMessage = "User name is required")]
    public string UserName { get; set; }

    [EmailAddress(ErrorMessage = "User Email should match Email format")]
    [Required(ErrorMessage = "User Email is required")]
    public string Email { get; set; }

    [Phone(ErrorMessage = "User Phone number should match phone number format")]
    [Required(ErrorMessage = "User Phone number is required")]
    public string PhoneNumber { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Confirm password is required")]
    [Compare(nameof(Password), ErrorMessage = "Confirm password does not match with Password.")]
    public string ConfirmPassword { get; set; }
}
