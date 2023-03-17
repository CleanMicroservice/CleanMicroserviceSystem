using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Metadata;

public class DefaultPackageMetadataService : IPackageMetadataService
{
    private readonly IPackageService _packages;
    private readonly RegistrationBuilder _builder;

    public DefaultPackageMetadataService(
        IPackageService packages,
        RegistrationBuilder builder)
    {
        this._packages = packages ?? throw new ArgumentNullException(nameof(packages));
        this._builder = builder ?? throw new ArgumentNullException(nameof(builder));
    }

    public async Task<BaGetRegistrationIndexResponse> GetRegistrationIndexOrNullAsync(
        string packageId,
        CancellationToken cancellationToken = default)
    {
        var packages = await this._packages.FindPackagesAsync(packageId, cancellationToken);
        return !packages.Any()
            ? null
            : this._builder.BuildIndex(
            new PackageRegistration(
                packageId,
                packages));
    }

    public async Task<RegistrationLeafResponse> GetRegistrationLeafOrNullAsync(
        string id,
        NuGetVersion version,
        CancellationToken cancellationToken = default)
    {
        var package = await this._packages.FindPackageOrNullAsync(id, version, cancellationToken);
        return package == null ? null : this._builder.BuildLeaf(package);
    }
}