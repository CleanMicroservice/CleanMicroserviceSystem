namespace CleanMicroserviceSystem.Authentication.Configurations;

public class JwtBearerConfiguration
{
    public string JwtSecurityKey { get; set; }

    public string JwtIssuer { get; set; }

    public string JwtAudience { get; set; }

    public int JwtExpiryForUser { get; set; }

    public int JwtExpiryForClient { get; set; }
}
