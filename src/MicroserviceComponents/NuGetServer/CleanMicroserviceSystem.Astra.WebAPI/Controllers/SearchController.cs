using BaGet.Core;
using BaGet.Protocol.Models;
using Microsoft.AspNetCore.Mvc;

namespace CleanMicroserviceSystem.Astra.WebAPI.Controllers;

[ApiController]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        this._searchService = searchService ?? throw new ArgumentNullException(nameof(searchService));
    }

    [HttpGet]
    [Route("v3/search")]
    public async Task<ActionResult<SearchResponse>> SearchAsync(
        [FromQuery(Name = "q")] string query = null,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20,
        [FromQuery] bool prerelease = false,
        [FromQuery] string semVerLevel = null,

        // These are unofficial parameters
        [FromQuery] string packageType = null,
        [FromQuery] string framework = null,
        CancellationToken cancellationToken = default)
    {
        var request = new SearchRequest
        {
            Skip = skip,
            Take = take,
            IncludePrerelease = prerelease,
            IncludeSemVer2 = semVerLevel == "2.0.0",
            PackageType = packageType,
            Framework = framework,
            Query = query ?? string.Empty,
        };

        return await this._searchService.SearchAsync(request, cancellationToken);
    }

    [HttpGet]
    [Route("v3/autocomplete")]
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
                IncludeSemVer2 = semVerLevel == "2.0.0",
                PackageId = versionsQuery,
            };

            return await this._searchService.ListPackageVersionsAsync(request, cancellationToken);
        }
        else
        {
            var request = new AutocompleteRequest
            {
                IncludePrerelease = prerelease,
                IncludeSemVer2 = semVerLevel == "2.0.0",
                PackageType = packageType,
                Skip = skip,
                Take = take,
                Query = autocompleteQuery,
            };

            return await this._searchService.AutocompleteAsync(request, cancellationToken);
        }
    }

    [HttpGet]
    [Route("v3/dependents")]
    public async Task<ActionResult<DependentsResponse>> DependentsAsync(
        [FromQuery] string packageId = null,
        CancellationToken cancellationToken = default)
    {
        return string.IsNullOrWhiteSpace(packageId)
            ? (ActionResult<DependentsResponse>)this.BadRequest()
            : (ActionResult<DependentsResponse>)await this._searchService.FindDependentsAsync(packageId, cancellationToken);
    }
}
