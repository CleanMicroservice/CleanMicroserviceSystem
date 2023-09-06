using CleanMicroserviceSystem.Aphrodite.Domain;

namespace CleanMicroserviceSystem.Aphrodite.Application.Configurations;

public class AppearanceConfiguration
{
    public const string ConfigurationKey = "AppearanceConfiguration";

    public string DefaultUriAfterLogin { get; set; } = RouterContract.StartUri;

    public bool SupportNTLM { get; set; } = false;
}
