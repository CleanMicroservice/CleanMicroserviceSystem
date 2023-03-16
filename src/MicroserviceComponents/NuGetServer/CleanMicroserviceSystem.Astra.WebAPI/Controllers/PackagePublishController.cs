using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Authentication;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Configuration;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Indexing;
using CleanMicroserviceSystem.Astra.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.WebAPI.Controllers;

[ApiController]
public class PackagePublishController : ControllerBase
{
    private readonly IAuthenticationService _authentication;
    private readonly IPackageIndexingService _indexer;
    private readonly IPackageDatabase _packages;
    private readonly IPackageDeletionService _deleteService;
    private readonly IOptionsSnapshot<BaGetOptions> _options;
    private readonly ILogger<PackagePublishController> _logger;

    public PackagePublishController(
        IAuthenticationService authentication,
        IPackageIndexingService indexer,
        IPackageDatabase packages,
        IPackageDeletionService deletionService,
        IOptionsSnapshot<BaGetOptions> options,
        ILogger<PackagePublishController> logger)
    {
        this._authentication = authentication ?? throw new ArgumentNullException(nameof(authentication));
        this._indexer = indexer ?? throw new ArgumentNullException(nameof(indexer));
        this._packages = packages ?? throw new ArgumentNullException(nameof(packages));
        this._deleteService = deletionService ?? throw new ArgumentNullException(nameof(deletionService));
        this._options = options ?? throw new ArgumentNullException(nameof(options));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPut]
    [Route("api/v2/package")]
    public async Task Upload(CancellationToken cancellationToken)
    {
        if (this._options.Value.IsReadOnlyMode ||
            !await this._authentication.AuthenticateAsync(this.Request.GetApiKey(), cancellationToken))
        {
            this.HttpContext.Response.StatusCode = 401;
            return;
        }

        try
        {
            using var uploadStream = await this.Request.GetUploadStreamOrNullAsync(cancellationToken);
            if (uploadStream == null)
            {
                this.HttpContext.Response.StatusCode = 400;
                return;
            }

            var result = await this._indexer.IndexAsync(uploadStream, cancellationToken);

            switch (result)
            {
                case PackageIndexingResult.InvalidPackage:
                    this.HttpContext.Response.StatusCode = 400;
                    break;

                case PackageIndexingResult.PackageAlreadyExists:
                    this.HttpContext.Response.StatusCode = 409;
                    break;

                case PackageIndexingResult.Success:
                    this.HttpContext.Response.StatusCode = 201;
                    break;
            }
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Exception thrown during package upload");

            this.HttpContext.Response.StatusCode = 500;
        }
    }

    [HttpDelete]
    [Route("api/v2/package/{id}/{version}")]
    public async Task<IActionResult> Delete(string id, string version, CancellationToken cancellationToken)
    {
        if (this._options.Value.IsReadOnlyMode)
            return this.Unauthorized();

        if (!NuGetVersion.TryParse(version, out var nugetVersion))
            return this.NotFound();

        if (!await this._authentication.AuthenticateAsync(this.Request.GetApiKey(), cancellationToken))
            return this.Unauthorized();

        return await this._deleteService.TryDeletePackageAsync(id, nugetVersion, cancellationToken) ? this.NoContent() : this.NotFound();
    }

    [HttpPost]
    [Route("api/v2/package/{id}/{version}")]
    public async Task<IActionResult> Relist(string id, string version, CancellationToken cancellationToken)
    {
        if (this._options.Value.IsReadOnlyMode)
            return this.Unauthorized();

        if (!NuGetVersion.TryParse(version, out var nugetVersion))
            return this.NotFound();

        if (!await this._authentication.AuthenticateAsync(this.Request.GetApiKey(), cancellationToken))
            return this.Unauthorized();

        return await this._packages.RelistPackageAsync(id, nugetVersion, cancellationToken) ? this.Ok() : (IActionResult)this.NotFound();
    }
}
