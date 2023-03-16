namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Search;

public class VersionsRequest
{
    public bool IncludePrerelease { get; set; }

    public bool IncludeSemVer2 { get; set; }

    public string PackageId { get; set; }
}