using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;
using NuGet.Frameworks;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Extensions;

public static class CatalogModelExtensions
{
    public static List<CatalogLeafItem> GetLeavesInBounds(
        this CatalogPage catalogPage,
        DateTimeOffset minCommitTimestamp,
        DateTimeOffset maxCommitTimestamp,
        bool excludeRedundantLeaves)
    {
        var leaves = catalogPage
            .Items
            .Where(x => x.CommitTimestamp > minCommitTimestamp && x.CommitTimestamp <= maxCommitTimestamp)
            .OrderBy(x => x.CommitTimestamp);

        if (excludeRedundantLeaves)
        {
            leaves = leaves
                .GroupBy(x => new
                {
                    PackageId = x.PackageId.ToLowerInvariant(),
                    PackageVersion = x.ParsePackageVersion()
                })
                .Select(x => x.Last())
                .OrderBy(x => x.CommitTimestamp);
        }

        return leaves
            .ThenBy(x => x.PackageId, StringComparer.OrdinalIgnoreCase)
            .ThenBy(x => x.ParsePackageVersion())
            .ToList();
    }

    public static List<CatalogPageItem> GetPagesInBounds(
        this CatalogIndex catalogIndex,
        DateTimeOffset minCommitTimestamp,
        DateTimeOffset maxCommitTimestamp)
    {
        return catalogIndex
            .GetPagesInBoundsLazy(minCommitTimestamp, maxCommitTimestamp)
            .ToList();
    }

    private static IEnumerable<CatalogPageItem> GetPagesInBoundsLazy(
        this CatalogIndex catalogIndex,
        DateTimeOffset minCommitTimestamp,
        DateTimeOffset maxCommitTimestamp)
    {
        var upperRange = catalogIndex
            .Items
            .Where(x => x.CommitTimestamp > minCommitTimestamp)
            .OrderBy(x => x.CommitTimestamp);

        foreach (var page in upperRange)
        {
            yield return page;

            if (page.CommitTimestamp > maxCommitTimestamp)
                break;
        }
    }

    public static NuGetVersion ParsePackageVersion(this ICatalogLeafItem leaf)
    {
        return NuGetVersion.Parse(leaf.PackageVersion);
    }

    public static NuGetFramework ParseTargetFramework(this DependencyGroupItem packageDependencyGroup)
    {
        return string.IsNullOrEmpty(packageDependencyGroup.TargetFramework)
            ? NuGetFramework.AnyFramework
            : NuGetFramework.Parse(packageDependencyGroup.TargetFramework);
    }

    public static VersionRange ParseRange(this DependencyItem packageDependency)
    {
        return !VersionRange.TryParse(packageDependency.Range, out var parsed) ? VersionRange.All : parsed;
    }

    public static bool IsPackageDelete(this CatalogLeafItem leaf)
    {
        return leaf.Type == "nuget:PackageDelete";
    }

    public static bool IsPackageDelete(this CatalogLeaf leaf)
    {
        return leaf.Type.FirstOrDefault() == "PackageDelete";
    }

    public static bool IsPackageDetails(this CatalogLeafItem leaf)
    {
        return leaf.Type == "nuget:PackageDetails";
    }

    public static bool IsPackageDetails(this CatalogLeaf leaf)
    {
        return leaf.Type.FirstOrDefault() == "PackageDetails";
    }

    public static bool IsListed(this PackageDetailsCatalogLeaf leaf)
    {
        if (leaf.Listed.HasValue)
            return leaf.Listed.Value;

        return leaf.Published.Year != 1900;
    }

    public static bool IsSemVer2(this PackageDetailsCatalogLeaf leaf)
    {
        var parsedPackageVersion = leaf.ParsePackageVersion();
        if (parsedPackageVersion.IsSemVer2)
            return true;

        if (leaf.VerbatimVersion != null)
        {
            var parsedVerbatimVersion = NuGetVersion.Parse(leaf.VerbatimVersion);
            if (parsedVerbatimVersion.IsSemVer2)
                return true;
        }

        if (leaf.DependencyGroups != null)
        {
            foreach (var dependencyGroup in leaf.DependencyGroups)
            {
                if (dependencyGroup.Dependencies == null)
                    continue;

                foreach (var dependency in dependencyGroup.Dependencies)
                {
                    var versionRange = dependency.ParseRange();
                    if ((versionRange.MaxVersion != null && versionRange.MaxVersion.IsSemVer2)
                        || (versionRange.MinVersion != null && versionRange.MinVersion.IsSemVer2))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}