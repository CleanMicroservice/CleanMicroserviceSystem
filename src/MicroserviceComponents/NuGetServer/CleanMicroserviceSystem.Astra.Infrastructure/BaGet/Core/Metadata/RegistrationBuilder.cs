using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Metadata;

public class RegistrationBuilder
{
    private readonly IUrlGenerator _url;

    public RegistrationBuilder(IUrlGenerator url)
    {
        this._url = url ?? throw new ArgumentNullException(nameof(url));
    }

    public virtual BaGetRegistrationIndexResponse BuildIndex(PackageRegistration registration)
    {
        var sortedPackages = registration.Packages.OrderBy(p => p.Version).ToList();

        return new BaGetRegistrationIndexResponse
        {
            RegistrationIndexUrl = this._url.GetRegistrationIndexUrl(registration.PackageId),
            Type = RegistrationIndexResponse.DefaultType,
            Count = 1,
            TotalDownloads = registration.Packages.Sum(p => p.Downloads),
            Pages = new[]
            {
                new BaGetRegistrationIndexPage
                {
                    RegistrationPageUrl = this._url.GetRegistrationIndexUrl(registration.PackageId),
                    Count = registration.Packages.Count(),
                    Lower = sortedPackages.First().Version.ToNormalizedString().ToLowerInvariant(),
                    Upper = sortedPackages.Last().Version.ToNormalizedString().ToLowerInvariant(),
                    ItemsOrNull = sortedPackages.Select(this.ToRegistrationIndexPageItem).ToList(),
                }
            }
        };
    }

    public virtual RegistrationLeafResponse BuildLeaf(Package package)
    {
        var id = package.Id;
        var version = package.Version;

        return new RegistrationLeafResponse
        {
            Type = RegistrationLeafResponse.DefaultType,
            Listed = package.Listed,
            Published = package.Published,
            RegistrationLeafUrl = this._url.GetRegistrationLeafUrl(id, version),
            PackageContentUrl = this._url.GetPackageDownloadUrl(id, version),
            RegistrationIndexUrl = this._url.GetRegistrationIndexUrl(id)
        };
    }

    private BaGetRegistrationIndexPageItem ToRegistrationIndexPageItem(Package package)
    {
        return new BaGetRegistrationIndexPageItem
        {
            RegistrationLeafUrl = this._url.GetRegistrationLeafUrl(package.Id, package.Version),
            PackageContentUrl = this._url.GetPackageDownloadUrl(package.Id, package.Version),
            PackageMetadata = new BaGetPackageMetadata
            {
                PackageId = package.Id,
                Version = package.Version.ToFullString(),
                Authors = string.Join(", ", package.Authors),
                Description = package.Description,
                Downloads = package.Downloads,
                HasReadme = package.HasReadme,
                IconUrl = package.HasEmbeddedIcon
                    ? this._url.GetPackageIconDownloadUrl(package.Id, package.Version)
                    : package.IconUrlString,
                Language = package.Language,
                LicenseUrl = package.LicenseUrlString,
                Listed = package.Listed,
                MinClientVersion = package.MinClientVersion,
                ReleaseNotes = package.ReleaseNotes,
                PackageContentUrl = this._url.GetPackageDownloadUrl(package.Id, package.Version),
                PackageTypes = package.PackageTypes.Select(t => t.Name).ToList(),
                ProjectUrl = package.ProjectUrlString,
                RepositoryUrl = package.RepositoryUrlString,
                RepositoryType = package.RepositoryType,
                Published = package.Published,
                RequireLicenseAcceptance = package.RequireLicenseAcceptance,
                Summary = package.Summary,
                Tags = package.Tags,
                Title = package.Title,
                DependencyGroups = this.ToDependencyGroups(package)
            },
        };
    }

    private IReadOnlyList<DependencyGroupItem> ToDependencyGroups(Package package)
    {
        return package.Dependencies
            .GroupBy(d => d.TargetFramework)
            .Select(group => new DependencyGroupItem
            {
                TargetFramework = group.Key,

                Dependencies = group
                    .Where(d => d.Id != null && d.VersionRange != null)
                    .Select(d => new DependencyItem
                    {
                        Id = d.Id,
                        Range = d.VersionRange
                    })
                    .ToList()
            })
            .ToList();
    }
}