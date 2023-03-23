namespace CleanMicroserviceSystem.Aphrodite.Application.Extensions;

public static class BoolExtension
{
    public static string ToStatus(this bool enabled) => enabled ? "Enabled" : "Disabled";
}
