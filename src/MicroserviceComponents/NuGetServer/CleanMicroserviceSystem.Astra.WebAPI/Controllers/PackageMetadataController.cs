using CleanMicroserviceSystem.Astra.Contract;
using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Metadata;
using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.WebAPI.Controllers;

[ApiController]
[Route("api/v3/[controller]")]
public class PackageMetadataController : ControllerBase
{
    private readonly IPackageMetadataService _metadata;

    public PackageMetadataController(IPackageMetadataService metadata)
    {
        this._metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
    }

    [HttpGet]
    [Route("{id}/index.json", Name = NuGetRouteContract.RegistrationIndexRouteName)]
    public async Task<ActionResult<BaGetRegistrationIndexResponse>> RegistrationIndexAsync(string id, CancellationToken cancellationToken)
    {
        var index = await this._metadata.GetRegistrationIndexOrNullAsync(id, cancellationToken);
        return index == null ? (ActionResult<BaGetRegistrationIndexResponse>)this.NotFound() : (ActionResult<BaGetRegistrationIndexResponse>)index;
    }

    [HttpGet]
    [Route("{id}/{version}.json", Name = NuGetRouteContract.RegistrationLeafRouteName)]
    public async Task<ActionResult<RegistrationLeafResponse>> RegistrationLeafAsync(string id, string version, CancellationToken cancellationToken)
    {
        if (!NuGetVersion.TryParse(version, out var nugetVersion))
            return this.NotFound();

        var leaf = await this._metadata.GetRegistrationLeafOrNullAsync(id, nugetVersion, cancellationToken);
        return leaf == null ? (ActionResult<RegistrationLeafResponse>)this.NotFound() : (ActionResult<RegistrationLeafResponse>)leaf;
    }
}
