using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Metadata;

public class PackageRegistration
{
    public PackageRegistration(
        string packageId,
        IReadOnlyList<Package> packages)
    {
        this.PackageId = packageId ?? throw new ArgumentNullException(nameof(packageId));
        this.Packages = packages ?? throw new ArgumentNullException(nameof(packages));
    }

    public string PackageId { get; }

    public IReadOnlyList<Package> Packages { get; }
}