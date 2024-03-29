using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Extensions;

public static class PackageContentModelExtensions
{
    public static IReadOnlyList<NuGetVersion> ParseVersions(this PackageVersionsResponse response)
    {
        return response
            .Versions
            .Select(NuGetVersion.Parse)
            .ToList();
    }
}