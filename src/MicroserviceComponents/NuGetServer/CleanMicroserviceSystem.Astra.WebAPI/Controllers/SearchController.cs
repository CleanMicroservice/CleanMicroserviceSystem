using CleanMicroserviceSystem.Astra.Contract;
using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Search;
using CleanMicroserviceSystem.Authentication.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanMicroserviceSystem.Astra.WebAPI.Controllers;

[ApiController]
[Route("v3/")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;
    private readonly IAuthorizationService authorizationService;

    public SearchController(
        ISearchService searchService,
        IAuthorizationService authorizationService)
    {
        this._searchService = searchService ?? throw new ArgumentNullException(nameof(searchService));
        this.authorizationService = authorizationService;
    }

    [HttpGet]
    [Route("search", Name = NuGetRouteContract.SearchRouteName)]
    public async Task<ActionResult<SearchResponse>> SearchAsync(
        [FromQuery(Name = "q")] string? query = null,
        [FromQuery] int? skip = null,
        [FromQuery] int? take = null,
        [FromQuery] bool prerelease = false,
        [FromQuery] string? semVerLevel = null,
        [FromQuery] string? packageType = null,
        [FromQuery] string? framework = null,
        CancellationToken cancellationToken = default)
    {
        var validatePolicyResult = await this.authorizationService.AuthorizeAsync(this.HttpContext.User, IdentityContract.AstraAPIDeletePolicy);
        var includeUnlisted = validatePolicyResult.Succeeded;
        var request = new SearchRequest
        {
            Skip = skip,
            Take = take,
            IncludeUnlisted = includeUnlisted,
            IncludePrerelease = prerelease,
            IncludeSemVer2 = semVerLevel == NuGetServerContract.SemVerLevel,
            PackageType = packageType,
            Framework = framework,
            Query = query ?? string.Empty,
        };

        return await this._searchService.SearchAsync(request, cancellationToken);
    }

    [HttpGet]
    [Route("autocomplete", Name = NuGetRouteContract.AutocompleteRouteName)]
    public async Task<ActionResult<AutocompleteResponse>> AutocompleteAsync(
        [FromQuery(Name = "q")] string autocompleteQuery = null,
        [FromQuery(Name = "id")] string versionsQuery = null,
        [FromQuery] bool prerelease = false,
        [FromQuery] string semVerLevel = null,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20,

        // These are unofficial parameters
        [FromQuery] string packageType = null,
        CancellationToken cancellationToken = default)
    {
        // If only "id" is provided, find package versions. Otherwise, find package IDs.
        if (versionsQuery != null && autocompleteQuery == null)
        {
            var request = new VersionsRequest
            {
                IncludePrerelease = prerelease,
                IncludeSemVer2 = semVerLevel == NuGetServerContract.SemVerLevel,
                PackageId = versionsQuery,
            };

            return await this._searchService.ListPackageVersionsAsync(request, cancellationToken);
        }
        else
        {
            var request = new AutocompleteRequest
            {
                IncludePrerelease = prerelease,
                IncludeSemVer2 = semVerLevel == NuGetServerContract.SemVerLevel,
                PackageType = packageType,
                Skip = skip,
                Take = take,
                Query = autocompleteQuery,
            };

            return await this._searchService.AutocompleteAsync(request, cancellationToken);
        }
    }

    [HttpGet]
    [Route("dependents", Name = NuGetRouteContract.DependentsRouteName)]
    public async Task<ActionResult<DependentsResponse>> DependentsAsync(
        [FromQuery] string packageId = null,
        CancellationToken cancellationToken = default)
    {
        return string.IsNullOrWhiteSpace(packageId)
            ? (ActionResult<DependentsResponse>)this.BadRequest()
            : (ActionResult<DependentsResponse>)await this._searchService.FindDependentsAsync(packageId, cancellationToken);
    }
}
