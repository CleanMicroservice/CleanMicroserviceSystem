using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;
using NuGet.Versioning;
using PackageMetadataModel = CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models.PackageMetadata;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Extensions;

public static class RegistrationModelExtensions
{
    public static NuGetVersion ParseVersion(this PackageMetadataModel package)
    {
        return NuGetVersion.Parse(package.Version);
    }

    public static bool IsListed(this PackageMetadataModel package)
    {
        if (package.Listed.HasValue)
            return package.Listed.Value;

        return package.Published.Year != 1900;
    }

    public static NuGetVersion ParseLower(this RegistrationIndexPage page)
    {
        return NuGetVersion.Parse(page.Lower);
    }

    public static NuGetVersion ParseUpper(this RegistrationIndexPage page)
    {
        return NuGetVersion.Parse(page.Upper);
    }
}