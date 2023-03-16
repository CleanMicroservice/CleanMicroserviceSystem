using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;
using NuGet.Common;
using NuGet.Packaging;
using NuGetPackageType = NuGet.Packaging.Core.PackageType;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Extensions;

public static class PackageArchiveReaderExtensions
{
    public static bool HasReadme(this PackageArchiveReader package)
    {
        return !string.IsNullOrEmpty(package.NuspecReader.GetReadme());
    }

    public static bool HasEmbeddedIcon(this PackageArchiveReader package)
    {
        return !string.IsNullOrEmpty(package.NuspecReader.GetIcon());
    }

    public static async Task<Stream> GetReadmeAsync(
        this PackageArchiveReader package,
        CancellationToken cancellationToken)
    {
        var readmePath = package.NuspecReader.GetReadme();
        return readmePath == null
            ? throw new InvalidOperationException("Package does not have a readme!")
            : await package.GetStreamAsync(readmePath, cancellationToken);
    }

    public static async Task<Stream> GetIconAsync(
        this PackageArchiveReader package,
        CancellationToken cancellationToken)
    {
        return await package.GetStreamAsync(
            PathUtility.StripLeadingDirectorySeparators(package.NuspecReader.GetIcon()),
            cancellationToken);
    }

    public static Package GetPackageMetadata(this PackageArchiveReader packageReader)
    {
        var nuspec = packageReader.NuspecReader;

        (var repositoryUri, var repositoryType) = GetRepositoryMetadata(nuspec);

        return new Package
        {
            Id = nuspec.GetId(),
            Version = nuspec.GetVersion(),
            Authors = ParseAuthors(nuspec.GetAuthors()),
            Description = nuspec.GetDescription(),
            HasReadme = packageReader.HasReadme(),
            HasEmbeddedIcon = packageReader.HasEmbeddedIcon(),
            IsPrerelease = nuspec.GetVersion().IsPrerelease,
            Language = nuspec.GetLanguage() ?? string.Empty,
            ReleaseNotes = nuspec.GetReleaseNotes() ?? string.Empty,
            Listed = true,
            MinClientVersion = nuspec.GetMinClientVersion()?.ToNormalizedString() ?? string.Empty,
            Published = DateTime.UtcNow,
            RequireLicenseAcceptance = nuspec.GetRequireLicenseAcceptance(),
            SemVerLevel = GetSemVerLevel(nuspec),
            Summary = nuspec.GetSummary(),
            Title = nuspec.GetTitle(),
            IconUrl = ParseUri(nuspec.GetIconUrl()),
            LicenseUrl = ParseUri(nuspec.GetLicenseUrl()),
            ProjectUrl = ParseUri(nuspec.GetProjectUrl()),
            RepositoryUrl = repositoryUri,
            RepositoryType = repositoryType,
            Dependencies = GetDependencies(nuspec),
            Tags = ParseTags(nuspec.GetTags()),
            PackageTypes = GetPackageTypes(nuspec),
            TargetFrameworks = GetTargetFrameworks(packageReader),
        };
    }

    private static SemVerLevel GetSemVerLevel(NuspecReader nuspec)
    {
        if (nuspec.GetVersion().IsSemVer2)
            return SemVerLevel.SemVer2;

        foreach (var dependencyGroup in nuspec.GetDependencyGroups())
        {
            foreach (var dependency in dependencyGroup.Packages)
            {
                if ((dependency.VersionRange.MinVersion != null && dependency.VersionRange.MinVersion.IsSemVer2)
                    || (dependency.VersionRange.MaxVersion != null && dependency.VersionRange.MaxVersion.IsSemVer2))
                {
                    return SemVerLevel.SemVer2;
                }
            }
        }

        return SemVerLevel.Unknown;
    }

    private static Uri ParseUri(string uriString)
    {
        return string.IsNullOrEmpty(uriString) ? null : new Uri(uriString);
    }

    private static string[] ParseAuthors(string authors)
    {
        return string.IsNullOrEmpty(authors)
            ? (new string[0])
            : authors.Split(new[] { ',', ';', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
    }

    private static string[] ParseTags(string tags)
    {
        return string.IsNullOrEmpty(tags)
            ? (new string[0])
            : tags.Split(new[] { ',', ';', ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
    }

    private static (Uri repositoryUrl, string repositoryType) GetRepositoryMetadata(NuspecReader nuspec)
    {
        var repository = nuspec.GetRepositoryMetadata();

        if (string.IsNullOrEmpty(repository?.Url) ||
            !Uri.TryCreate(repository.Url, UriKind.Absolute, out var repositoryUri))
        {
            return (null, null);
        }

        if (repositoryUri.Scheme != Uri.UriSchemeHttps)
            return (null, null);

        return repository.Type.Length > 100
            ? throw new InvalidOperationException("Repository type must be less than or equal 100 characters")
            : ((Uri repositoryUrl, string repositoryType))(repositoryUri, repository.Type);
    }

    private static List<PackageDependency> GetDependencies(NuspecReader nuspec)
    {
        var dependencies = new List<PackageDependency>();

        foreach (var group in nuspec.GetDependencyGroups())
        {
            var targetFramework = group.TargetFramework.GetShortFolderName();

            if (!group.Packages.Any())
            {
                dependencies.Add(new PackageDependency
                {
                    Id = null,
                    VersionRange = null,
                    TargetFramework = targetFramework,
                });
            }

            foreach (var dependency in group.Packages)
            {
                dependencies.Add(new PackageDependency
                {
                    Id = dependency.Id,
                    VersionRange = dependency.VersionRange?.ToString(),
                    TargetFramework = targetFramework,
                });
            }
        }

        return dependencies;
    }

    private static List<PackageType> GetPackageTypes(NuspecReader nuspec)
    {
        var packageTypes = nuspec
            .GetPackageTypes()
            .Select(t => new PackageType
            {
                Name = t.Name,
                Version = t.Version.ToString()
            })
            .ToList();

        if (packageTypes.Count == 0)
        {
            packageTypes.Add(new PackageType
            {
                Name = NuGetPackageType.Dependency.Name,
                Version = NuGetPackageType.Dependency.Version.ToString(),
            });
        }

        return packageTypes;
    }

    private static List<TargetFramework> GetTargetFrameworks(PackageArchiveReader packageReader)
    {
        var targetFrameworks = packageReader
            .GetSupportedFrameworks()
            .Select(f => new TargetFramework
            {
                Moniker = f.GetShortFolderName()
            })
            .ToList();

        if (targetFrameworks.Count == 0)
            targetFrameworks.Add(new TargetFramework { Moniker = "any" });

        return targetFrameworks;
    }
}