using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Extensions;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Catalog;

public class CatalogProcessor
{
    private readonly ICatalogLeafProcessor _leafProcessor;
    private readonly ICatalogClient _client;
    private readonly ICursor _cursor;
    private readonly CatalogProcessorOptions _options;
    private readonly ILogger<CatalogProcessor> _logger;

    public CatalogProcessor(
        ICursor cursor,
        ICatalogClient client,
        ICatalogLeafProcessor leafProcessor,
        CatalogProcessorOptions options,
        ILogger<CatalogProcessor> logger)
    {
        this._leafProcessor = leafProcessor ?? throw new ArgumentNullException(nameof(leafProcessor));
        this._client = client ?? throw new ArgumentNullException(nameof(client));
        this._cursor = cursor ?? throw new ArgumentNullException(nameof(cursor));
        this._options = options ?? throw new ArgumentNullException(nameof(options));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<bool> ProcessAsync(CancellationToken cancellationToken = default)
    {
        var minCommitTimestamp = await this.GetMinCommitTimestamp(cancellationToken);
        this._logger.LogInformation(
            "Using time bounds {min:O} (exclusive) to {max:O} (inclusive).",
            minCommitTimestamp,
            this._options.MaxCommitTimestamp);

        return await this.ProcessIndexAsync(minCommitTimestamp, cancellationToken);
    }

    private async Task<bool> ProcessIndexAsync(DateTimeOffset minCommitTimestamp, CancellationToken cancellationToken)
    {
        var index = await this._client.GetIndexAsync(cancellationToken);

        var pageItems = index.GetPagesInBounds(
            minCommitTimestamp,
            this._options.MaxCommitTimestamp);
        this._logger.LogInformation(
            "{pages} pages were in the time bounds, out of {totalPages}.",
            pageItems.Count,
            index.Items.Count);

        var success = true;
        for (var i = 0; i < pageItems.Count; i++)
        {
            success = await this.ProcessPageAsync(minCommitTimestamp, pageItems[i], cancellationToken);
            if (!success)
            {
                this._logger.LogWarning(
                    "{unprocessedPages} out of {pages} pages were left incomplete due to a processing failure.",
                    pageItems.Count - i,
                    pageItems.Count);
                break;
            }
        }

        return success;
    }

    private async Task<bool> ProcessPageAsync(
        DateTimeOffset minCommitTimestamp,
        CatalogPageItem pageItem,
        CancellationToken cancellationToken)
    {
        var page = await this._client.GetPageAsync(pageItem.CatalogPageUrl, cancellationToken);

        var leafItems = page.GetLeavesInBounds(
            minCommitTimestamp,
            this._options.MaxCommitTimestamp,
            this._options.ExcludeRedundantLeaves);
        this._logger.LogInformation(
            "On page {page}, {leaves} out of {totalLeaves} were in the time bounds.",
            pageItem.CatalogPageUrl,
            leafItems.Count,
            page.Items.Count);

        DateTimeOffset? newCursor = null;
        var success = true;
        for (var i = 0; i < leafItems.Count; i++)
        {
            var leafItem = leafItems[i];

            if (newCursor.HasValue && newCursor.Value != leafItem.CommitTimestamp)
                await this._cursor.SetAsync(newCursor.Value, cancellationToken);

            newCursor = leafItem.CommitTimestamp;

            success = await this.ProcessLeafAsync(leafItem, cancellationToken);
            if (!success)
            {
                this._logger.LogWarning(
                    "{unprocessedLeaves} out of {leaves} leaves were left incomplete due to a processing failure.",
                    leafItems.Count - i,
                    leafItems.Count);
                break;
            }
        }

        if (newCursor.HasValue && success)
            await this._cursor.SetAsync(newCursor.Value);

        return success;
    }

    private async Task<bool> ProcessLeafAsync(CatalogLeafItem leafItem, CancellationToken cancellationToken)
    {
        bool success;
        try
        {
            if (leafItem.IsPackageDelete())
            {
                var packageDelete = await this._client.GetPackageDeleteLeafAsync(leafItem.CatalogLeafUrl);
                success = await this._leafProcessor.ProcessPackageDeleteAsync(packageDelete, cancellationToken);
            }
            else if (leafItem.IsPackageDetails())
            {
                var packageDetails = await this._client.GetPackageDetailsLeafAsync(leafItem.CatalogLeafUrl);
                success = await this._leafProcessor.ProcessPackageDetailsAsync(packageDetails, cancellationToken);
            }
            else
            {
                throw new NotSupportedException($"The catalog leaf type '{leafItem.Type}' is not supported.");
            }
        }
        catch (Exception exception)
        {
            this._logger.LogError(
                0,
                exception,
                "An exception was thrown while processing leaf {leafUrl}.",
                leafItem.CatalogLeafUrl);
            success = false;
        }

        if (!success)
        {
            this._logger.LogWarning(
                "Failed to process leaf {leafUrl} ({packageId} {packageVersion}, {leafType}).",
                leafItem.CatalogLeafUrl,
                leafItem.PackageId,
                leafItem.PackageVersion,
                leafItem.Type);
        }

        return success;
    }

    private async Task<DateTimeOffset> GetMinCommitTimestamp(CancellationToken cancellationToken)
    {
        var minCommitTimestamp = await this._cursor.GetAsync(cancellationToken);

        minCommitTimestamp ??= this._options.DefaultMinCommitTimestamp
            ?? this._options.MinCommitTimestamp;

        if (minCommitTimestamp.Value < this._options.MinCommitTimestamp)
            minCommitTimestamp = this._options.MinCommitTimestamp;

        return minCommitTimestamp.Value;
    }
}