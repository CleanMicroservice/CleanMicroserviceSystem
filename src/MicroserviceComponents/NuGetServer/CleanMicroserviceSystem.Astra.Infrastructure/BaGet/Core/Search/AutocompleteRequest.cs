namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Search;

public class AutocompleteRequest
{
    public int Skip { get; set; }

    public int Take { get; set; }

    public bool IncludePrerelease { get; set; }

    public bool IncludeSemVer2 { get; set; }

    public string PackageType { get; set; }

    public string Query { get; set; }
}