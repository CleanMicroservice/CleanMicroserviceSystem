using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

public class PackageNotFoundException : Exception
{
    public PackageNotFoundException(string packageId, NuGetVersion packageVersion)
        : base($"Could not find package {packageId} {packageVersion}")
    {
        this.PackageId = packageId ?? throw new ArgumentNullException(nameof(packageId));
        this.PackageVersion = packageVersion ?? throw new ArgumentNullException(nameof(packageVersion));
    }

    public string PackageId { get; }

    public NuGetVersion PackageVersion { get; }
}