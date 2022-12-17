namespace CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Users;

public class UserSearchRequest
{
    public int? Id { get; set; } = default;
    
    public string? UserName { get; set; } = default;
    
    public string? Email { get; set; } = default;
    
    public string? PhoneNumber { get; set; } = default;
    
    public int Start { get; set; } = 0;
    
    public int Count { get; set; } = 10;
}
