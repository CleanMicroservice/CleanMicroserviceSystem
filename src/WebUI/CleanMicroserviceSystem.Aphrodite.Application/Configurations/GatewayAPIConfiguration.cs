namespace CleanMicroserviceSystem.Aphrodite.Application.Configurations;

public class GatewayAPIConfiguration
{
    public string GatewayBaseAddress { get; set; }

    public int TokenExpiryInMinutes { get; set; }

    public int TokenRefreshInMinutes { get; set; }
}
