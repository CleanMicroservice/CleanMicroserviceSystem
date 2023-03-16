using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Metadata;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.WebAPI.Controllers;

[ApiController]
public class PackageMetadataController : ControllerBase
{
    private readonly IPackageMetadataService _metadata;

    public PackageMetadataController(IPackageMetadataService metadata)
    {
        this._metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
    }

    [HttpGet]
    [Route("v3/registration/{id}/index.json")]
    public async Task<ActionResult<BaGetRegistrationIndexResponse>> RegistrationIndexAsync(string id, CancellationToken cancellationToken)
    {
        var index = await this._metadata.GetRegistrationIndexOrNullAsync(id, cancellationToken);
        return index == null ? (ActionResult<BaGetRegistrationIndexResponse>)this.NotFound() : (ActionResult<BaGetRegistrationIndexResponse>)index;
    }

    [HttpGet]
    [Route("v3/registration/{id}/{version}.json")]
    public async Task<ActionResult<RegistrationLeafResponse>> RegistrationLeafAsync(string id, string version, CancellationToken cancellationToken)
    {
        if (!NuGetVersion.TryParse(version, out var nugetVersion))
            return this.NotFound();

        var leaf = await this._metadata.GetRegistrationLeafOrNullAsync(id, nugetVersion, cancellationToken);
        return leaf == null ? (ActionResult<RegistrationLeafResponse>)this.NotFound() : (ActionResult<RegistrationLeafResponse>)leaf;
    }
}
