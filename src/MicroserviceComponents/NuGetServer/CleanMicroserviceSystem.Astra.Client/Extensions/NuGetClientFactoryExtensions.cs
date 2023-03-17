using CleanMicroserviceSystem.Astra.Client.Catalog;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Astra.Client.Extensions;

public static class NuGetClientFactoryExtensions
{
    public static CatalogProcessor CreateCatalogProcessor(
        this AstraNuGetClientFactory clientFactory,
        ICursor cursor,
        ICatalogLeafProcessor leafProcessor,
        CatalogProcessorOptions options,
        ILogger<CatalogProcessor> logger)
    {
        var catalogClient = clientFactory.CreateCatalogClient();

        return new CatalogProcessor(
            cursor,
            catalogClient,
            leafProcessor,
            options,
            logger);
    }
}