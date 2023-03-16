using CleanMicroserviceSystem.Astra.Domain;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.Services;

public class BaGetUrlGenerator : IUrlGenerator
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly LinkGenerator _linkGenerator;

    public BaGetUrlGenerator(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
    {
        this._httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this._linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
    }

    public string GetServiceIndexUrl()
    {
        return this._linkGenerator.GetUriByRouteValues(
            this._httpContextAccessor.HttpContext,
            NuGetRouteContract.IndexRouteName,
            values: null);
    }

    public string GetPackageContentResourceUrl()
    {
        return this.AbsoluteUrl("v3/package");
    }

    public string GetPackageMetadataResourceUrl()
    {
        return this.AbsoluteUrl("v3/registration");
    }

    public string GetPackagePublishResourceUrl()
    {
        return this._linkGenerator.GetUriByRouteValues(
            this._httpContextAccessor.HttpContext,
            NuGetRouteContract.UploadPackageRouteName,
            values: null);
    }

    public string GetSymbolPublishResourceUrl()
    {
        return this._linkGenerator.GetUriByRouteValues(
            this._httpContextAccessor.HttpContext,
            NuGetRouteContract.UploadSymbolRouteName,
            values: null);
    }

    public string GetSearchResourceUrl()
    {
        return this._linkGenerator.GetUriByRouteValues(
            this._httpContextAccessor.HttpContext,
            NuGetRouteContract.SearchRouteName,
            values: null);
    }

    public string GetAutocompleteResourceUrl()
    {
        return this._linkGenerator.GetUriByRouteValues(
            this._httpContextAccessor.HttpContext,
            NuGetRouteContract.AutocompleteRouteName,
            values: null);
    }

    public string GetRegistrationIndexUrl(string id)
    {
        return this._linkGenerator.GetUriByRouteValues(
            this._httpContextAccessor.HttpContext,
            NuGetRouteContract.RegistrationIndexRouteName,
            values: new { Id = id.ToLowerInvariant() });
    }

    public string GetRegistrationPageUrl(string id, NuGetVersion lower, NuGetVersion upper)
    {
        // BaGet does not support paging the registration resource.
        throw new NotImplementedException();
    }

    public string GetRegistrationLeafUrl(string id, NuGetVersion version)
    {
        return this._linkGenerator.GetUriByRouteValues(
            this._httpContextAccessor.HttpContext,
            NuGetRouteContract.RegistrationLeafRouteName,
            values: new
            {
                Id = id.ToLowerInvariant(),
                Version = version.ToNormalizedString().ToLowerInvariant(),
            });
    }

    public string GetPackageVersionsUrl(string id)
    {
        return this._linkGenerator.GetUriByRouteValues(
            this._httpContextAccessor.HttpContext,
            NuGetRouteContract.PackageVersionsRouteName,
            values: new { Id = id.ToLowerInvariant() });
    }

    public string GetPackageDownloadUrl(string id, NuGetVersion version)
    {
        id = id.ToLowerInvariant();
        var versionString = version.ToNormalizedString().ToLowerInvariant();

        return this._linkGenerator.GetUriByRouteValues(
            this._httpContextAccessor.HttpContext,
            NuGetRouteContract.PackageDownloadRouteName,
            values: new
            {
                Id = id,
                Version = versionString,
                IdVersion = $"{id}.{versionString}"
            });
    }

    public string GetPackageManifestDownloadUrl(string id, NuGetVersion version)
    {
        id = id.ToLowerInvariant();
        var versionString = version.ToNormalizedString().ToLowerInvariant();

        return this._linkGenerator.GetUriByRouteValues(
            this._httpContextAccessor.HttpContext,
            NuGetRouteContract.PackageDownloadRouteName,
            values: new
            {
                Id = id,
                Version = versionString,
                Id2 = id,
            });
    }

    public string GetPackageIconDownloadUrl(string id, NuGetVersion version)
    {
        id = id.ToLowerInvariant();
        var versionString = version.ToNormalizedString().ToLowerInvariant();

        return this._linkGenerator.GetUriByRouteValues(
            this._httpContextAccessor.HttpContext,
            NuGetRouteContract.PackageDownloadIconRouteName,
            values: new
            {
                Id = id,
                Version = versionString
            });
    }

    private string AbsoluteUrl(string relativePath)
    {
        var request = this._httpContextAccessor.HttpContext.Request;

        return string.Concat(
            request.Scheme,
            "://",
            request.Host.ToUriComponent(),
            request.PathBase.ToUriComponent(),
            "/",
            relativePath);
    }
}
