namespace CleanMicroserviceSystem.Authentication.Configurations;

public class JwtBearerConfiguration
{
    public string JwtSecurityKey { get; set; } = null!;

    public string JwtIssuer { get; set; } = null!;

    public string JwtAudience { get; set; } = null!;

    public int JwtExpiryForUser { get; set; }

    public int JwtExpiryForClient { get; set; }
}
