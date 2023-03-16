using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Catalog;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Extensions;

public static class NuGetClientFactoryExtensions
{
    public static CatalogProcessor CreateCatalogProcessor(
        this NuGetClientFactory clientFactory,
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