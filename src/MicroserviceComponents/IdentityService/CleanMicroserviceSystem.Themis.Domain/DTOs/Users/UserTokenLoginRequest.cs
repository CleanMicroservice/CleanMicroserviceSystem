using System.ComponentModel.DataAnnotations;

namespace CleanMicroserviceSystem.Themis.Domain.DTOs.Users;

public class UserTokenLoginRequest
{
    [Required(ErrorMessage = "User name is required")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}
