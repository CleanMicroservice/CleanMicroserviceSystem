using System.ComponentModel.DataAnnotations;

namespace CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Users;
public class UserUpdateRequest
{
    public string? UserName { get; set; }

    [EmailAddress(ErrorMessage = "User Email should match Email format")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "User Phone number should match phone number format")]
    public string? PhoneNumber { get; set; }
}
