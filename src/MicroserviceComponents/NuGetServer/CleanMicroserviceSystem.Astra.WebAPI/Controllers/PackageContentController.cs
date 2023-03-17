using CleanMicroserviceSystem.Astra.Contract;
using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Content;

using CleanMicroserviceSystem.Oceanus.Application.Abstraction.Attributes;
using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.WebAPI.Controllers;

[ApiController]
[Route("api/v3/[controller]")]
public class PackageContentController : ControllerBase
{
    private readonly IPackageContentService _content;

    public PackageContentController(IPackageContentService content)
    {
        this._content = content ?? throw new ArgumentNullException(nameof(content));
    }

    [HttpGet]
    [Route("{id}/index.json", Name = NuGetRouteContract.PackageVersionsRouteName)]
    public async Task<ActionResult<PackageVersionsResponse>> GetPackageVersionsAsync(string id, CancellationToken cancellationToken)
    {
        var versions = await this._content.GetPackageVersionsOrNullAsync(id, cancellationToken);
        return versions == null ? (ActionResult<PackageVersionsResponse>)this.NotFound() : (ActionResult<PackageVersionsResponse>)versions;
    }

    [HttpGet]
    [WebAPILogActionFilter(true, false)]
    [Route("{id}/{version}/{idVersion}.nupkg", Name = NuGetRouteContract.PackageDownloadRouteName)]
    public async Task<IActionResult> DownloadPackageAsync(string id, string version, CancellationToken cancellationToken)
    {
        if (!NuGetVersion.TryParse(version, out var nugetVersion))
            return this.NotFound();

        var packageStream = await this._content.GetPackageContentStreamOrNullAsync(id, nugetVersion, cancellationToken);
        return packageStream == null ? this.NotFound() : this.File(packageStream, "application/octet-stream");
    }

    [HttpGet]
    [WebAPILogActionFilter(true, false)]
    [Route("{id}/{version}/{id2}.nuspec", Name = NuGetRouteContract.PackageDownloadManifestRouteName)]
    public async Task<IActionResult> DownloadNuspecAsync(string id, string version, CancellationToken cancellationToken)
    {
        if (!NuGetVersion.TryParse(version, out var nugetVersion))
            return this.NotFound();

        var nuspecStream = await this._content.GetPackageManifestStreamOrNullAsync(id, nugetVersion, cancellationToken);
        return nuspecStream == null ? this.NotFound() : this.File(nuspecStream, "text/xml");
    }

    [HttpGet]
    [WebAPILogActionFilter(true, false)]
    [Route("{id}/{version}/readme", Name = NuGetRouteContract.PackageDownloadReadmeRouteName)]
    public async Task<IActionResult> DownloadReadmeAsync(string id, string version, CancellationToken cancellationToken)
    {
        if (!NuGetVersion.TryParse(version, out var nugetVersion))
            return this.NotFound();

        var readmeStream = await this._content.GetPackageReadmeStreamOrNullAsync(id, nugetVersion, cancellationToken);
        return readmeStream == null ? this.NotFound() : this.File(readmeStream, "text/markdown");
    }

    [HttpGet]
    [WebAPILogActionFilter(true, false)]
    [Route("{id}/{version}/icon", Name = NuGetRouteContract.PackageDownloadIconRouteName)]
    public async Task<IActionResult> DownloadIconAsync(string id, string version, CancellationToken cancellationToken)
    {
        if (!NuGetVersion.TryParse(version, out var nugetVersion))
            return this.NotFound();

        var iconStream = await this._content.GetPackageIconStreamOrNullAsync(id, nugetVersion, cancellationToken);
        return iconStream == null ? this.NotFound() : this.File(iconStream, "image/xyz");
    }
}
