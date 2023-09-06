namespace CleanMicroserviceSystem.Aphrodite.Application.Configurations;

public class GatewayAPIConfiguration
{
    public const string ConfigurationKey = "GatewayAPIConfiguration";

    public string GatewayBaseAddress { get; set; } = default!;

    public string NTLMBaseAddress { get; set; } = default!;

    public int TokenExpiryInMinutes { get; set; }

    public int TokenRefreshInMinutes { get; set; }
}
