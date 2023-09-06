namespace CleanMicroserviceSystem.Aphrodite.Application.Configurations;

public class NuGetServerConfiguration
{
    public const string ConfigurationKey = "NuGetServerConfiguration";

    public string ApiKey { get; set; } = default!;
}
