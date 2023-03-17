namespace CleanMicroserviceSystem.Astra.Client.Catalog;

public class CatalogProcessorOptions
{
    public DateTimeOffset? DefaultMinCommitTimestamp { get; set; }

    public DateTimeOffset MinCommitTimestamp { get; set; }

    public DateTimeOffset MaxCommitTimestamp { get; set; }

    public bool ExcludeRedundantLeaves { get; set; }
}