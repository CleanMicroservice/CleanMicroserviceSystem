using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Extensions;

public static class SearchModelExtensions
{
    public static NuGetVersion ParseVersion(this SearchResult result)
    {
        return NuGetVersion.Parse(result.Version);
    }

    public static NuGetVersion ParseVersion(this SearchResultVersion result)
    {
        return NuGetVersion.Parse(result.Version);
    }
}