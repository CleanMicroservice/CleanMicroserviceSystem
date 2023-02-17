namespace CleanMicroserviceSystem.Authentication.Configurations;

public class JwtBearerConfiguration
{
    public string JwtSecurityKey { get; set; }

    public string JwtIssuer { get; set; }

    public string JwtAudience { get; set; }

    public int JwtExpiryInMinutes { get; set; }
}
