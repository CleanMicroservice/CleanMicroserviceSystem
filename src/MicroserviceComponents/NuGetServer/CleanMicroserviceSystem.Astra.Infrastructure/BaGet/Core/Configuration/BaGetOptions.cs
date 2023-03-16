namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Configuration;

public class BaGetOptions
{
    public string ApiKey { get; set; }

    public string PathBase { get; set; }

    public bool RunMigrationsAtStartup { get; set; } = true;

    public PackageDeletionBehavior PackageDeletionBehavior { get; set; } = PackageDeletionBehavior.Unlist;

    public bool AllowPackageOverwrites { get; set; } = false;

    public bool IsReadOnlyMode { get; set; } = false;

    public string Urls { get; set; }

    public DatabaseOptions Database { get; set; }

    public StorageOptions Storage { get; set; }

    public SearchOptions Search { get; set; }

    public MirrorOptions Mirror { get; set; }
}