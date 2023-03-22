using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Indexing;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Metadata;
using Microsoft.EntityFrameworkCore;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Search;

public class DatabaseSearchService : ISearchService
{
    private readonly IContext _context;
    private readonly IFrameworkCompatibilityService _frameworks;
    private readonly ISearchResponseBuilder _searchBuilder;

    public DatabaseSearchService(
        IContext context,
        IFrameworkCompatibilityService frameworks,
        ISearchResponseBuilder searchBuilder)
    {
        this._context = context ?? throw new ArgumentNullException(nameof(context));
        this._frameworks = frameworks ?? throw new ArgumentNullException(nameof(frameworks));
        this._searchBuilder = searchBuilder ?? throw new ArgumentNullException(nameof(searchBuilder));
    }

    public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        var frameworks = this.GetCompatibleFrameworksOrNull(request.Framework);

        IQueryable<Package> search = this._context.Packages;
        search = this.ApplySearchQuery(search, request.Query);
        search = this.ApplySearchFilters(
            search,
            request.IncludePrerelease,
            request.IncludeSemVer2,
            request.PackageType,
            frameworks);

        var packageIds = search
            .Select(p => p.Id)
            .Distinct();
        packageIds = packageIds.OrderBy(id => id);

        if (request.Skip.HasValue)
            packageIds = packageIds.Skip(request.Skip.Value);
        if (request.Take.HasValue)
            packageIds = packageIds.Take(request.Take.Value);

        if (this._context.SupportsLimitInSubqueries)
        {
            search = this._context.Packages.Where(p => packageIds.Contains(p.Id));
        }
        else
        {
            var packageIdResults = await packageIds.ToListAsync(cancellationToken);

            search = this._context.Packages.Where(p => packageIdResults.Contains(p.Id));
        }

        search = this.ApplySearchFilters(
            search,
            request.IncludePrerelease,
            request.IncludeSemVer2,
            request.PackageType,
            frameworks);

        var results = await search.ToListAsync(cancellationToken);
        var groupedResults = results
            .GroupBy(p => p.Id, StringComparer.OrdinalIgnoreCase)
            .Select(group => new PackageRegistration(group.Key, group.ToList()))
            .ToList();

        return this._searchBuilder.BuildSearch(groupedResults);
    }

    public async Task<AutocompleteResponse> AutocompleteAsync(
        AutocompleteRequest request,
        CancellationToken cancellationToken)
    {
        IQueryable<Package> search = this._context.Packages;

        search = this.ApplySearchQuery(search, request.Query);
        search = this.ApplySearchFilters(
            search,
            request.IncludePrerelease,
            request.IncludeSemVer2,
            request.PackageType,
            frameworks: null);

        var packageIds = await search
            .OrderByDescending(p => p.Downloads)
            .Select(p => p.Id)
            .Distinct()
            .Skip(request.Skip)
            .Take(request.Take)
            .ToListAsync(cancellationToken);

        return this._searchBuilder.BuildAutocomplete(packageIds);
    }

    public async Task<AutocompleteResponse> ListPackageVersionsAsync(
        VersionsRequest request,
        CancellationToken cancellationToken)
    {
        var packageId = request.PackageId.ToLower();
        var search = this._context
            .Packages
            .Where(p => p.Id.ToLower().Equals(packageId));

        search = this.ApplySearchFilters(
            search,
            request.IncludePrerelease,
            request.IncludeSemVer2,
            packageType: null,
            frameworks: null);

        var packageVersions = await search
            .Select(p => p.NormalizedVersionString)
            .ToListAsync(cancellationToken);

        return this._searchBuilder.BuildAutocomplete(packageVersions);
    }

    public async Task<DependentsResponse> FindDependentsAsync(string packageId, CancellationToken cancellationToken)
    {
        var dependents = await this._context
            .Packages
            .Where(p => p.Listed)
            .OrderByDescending(p => p.Downloads)
            .Where(p => p.Dependencies.Any(d => d.Id == packageId))
            .Take(20)
            .Select(r => new PackageDependent
            {
                Id = r.Id,
                Description = r.Description,
                TotalDownloads = r.Downloads
            })
            .Distinct()
            .ToListAsync(cancellationToken);

        return this._searchBuilder.BuildDependents(dependents);
    }

    private IQueryable<Package> ApplySearchQuery(IQueryable<Package> query, string? search)
    {
        if (string.IsNullOrEmpty(search))
            return query;

        search = search.ToLowerInvariant();

        return query.Where(p => p.Id.ToLower().Contains(search));
    }

    private IQueryable<Package> ApplySearchFilters(
        IQueryable<Package> query,
        bool includePrerelease,
        bool includeSemVer2,
        string? packageType,
        IReadOnlyList<string>? frameworks)
    {
        if (!includePrerelease)
            query = query.Where(p => !p.IsPrerelease);

        if (!includeSemVer2)
            query = query.Where(p => p.SemVerLevel != SemVerLevel.SemVer2);

        if (!string.IsNullOrEmpty(packageType))
            query = query.Where(p => p.PackageTypes.Any(t => t.Name == packageType));

        if (frameworks != null)
            query = query.Where(p => p.TargetFrameworks.Any(f => frameworks.Contains(f.Moniker)));

        return query.Where(p => p.Listed);
    }

    private IReadOnlyList<string>? GetCompatibleFrameworksOrNull(string? framework)
    {
        return framework is null ? null : this._frameworks.FindAllCompatibleFrameworks(framework!);
    }
}