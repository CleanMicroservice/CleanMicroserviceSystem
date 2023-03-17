namespace CleanMicroserviceSystem.Astra.Contract.NuGetPackages;


public interface ICatalogLeafItem
{
    DateTimeOffset CommitTimestamp { get; }

    string PackageId { get; }

    string PackageVersion { get; }
}