using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;

public class Package
{
    public int Key { get; set; }

    public string Id { get; set; }

    public NuGetVersion Version
    {
        get =>
            NuGetVersion.Parse(
                this.OriginalVersionString ?? this.NormalizedVersionString);

        set
        {
            this.NormalizedVersionString = value.ToNormalizedString().ToLowerInvariant();
            this.OriginalVersionString = value.OriginalVersion;
        }
    }

    public string[] Authors { get; set; }
    public string Description { get; set; }
    public long Downloads { get; set; }
    public bool HasReadme { get; set; }
    public bool HasEmbeddedIcon { get; set; }
    public bool IsPrerelease { get; set; }
    public string ReleaseNotes { get; set; }
    public string Language { get; set; }
    public bool Listed { get; set; }
    public string MinClientVersion { get; set; }
    public DateTime Published { get; set; }
    public bool RequireLicenseAcceptance { get; set; }
    public SemVerLevel SemVerLevel { get; set; }
    public string Summary { get; set; }
    public string Title { get; set; }

    public Uri IconUrl { get; set; }
    public Uri LicenseUrl { get; set; }
    public Uri ProjectUrl { get; set; }

    public Uri RepositoryUrl { get; set; }
    public string RepositoryType { get; set; }

    public string[] Tags { get; set; }

    public byte[] RowVersion { get; set; }

    public virtual List<PackageDependency> Dependencies { get; set; }
    public virtual List<PackageType> PackageTypes { get; set; }
    public virtual List<TargetFramework> TargetFrameworks { get; set; }

    public string NormalizedVersionString { get; set; }
    public string OriginalVersionString { get; set; }

    public string IconUrlString => this.IconUrl?.AbsoluteUri ?? string.Empty;
    public string LicenseUrlString => this.LicenseUrl?.AbsoluteUri ?? string.Empty;
    public string ProjectUrlString => this.ProjectUrl?.AbsoluteUri ?? string.Empty;
    public string RepositoryUrlString => this.RepositoryUrl?.AbsoluteUri ?? string.Empty;
}