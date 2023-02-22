namespace CleanMicroserviceSystem.Themis.Domain.DTOs.Users;

public class UserInformationResponse
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }
}
