namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;


public interface ICatalogLeafItem
{
    DateTimeOffset CommitTimestamp { get; }

    string PackageId { get; }

    string PackageVersion { get; }
}