using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Upstream;

public class DownloadsImporter
{
    private const int BatchSize = 200;

    private readonly IContext _context;
    private readonly IPackageDownloadsSource _downloadsSource;
    private readonly ILogger<DownloadsImporter> _logger;

    public DownloadsImporter(
        IContext context,
        IPackageDownloadsSource downloadsSource,
        ILogger<DownloadsImporter> logger)
    {
        this._context = context ?? throw new ArgumentNullException(nameof(context));
        this._downloadsSource = downloadsSource ?? throw new ArgumentNullException(nameof(downloadsSource));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task ImportAsync(CancellationToken cancellationToken)
    {
        var packageDownloads = await this._downloadsSource.GetPackageDownloadsAsync();
        var packages = await this._context.Packages.CountAsync();
        var batches = (packages / BatchSize) + 1;

        for (var batch = 0; batch < batches; batch++)
        {
            this._logger.LogInformation("Importing batch {Batch}...", batch);

            foreach (var package in await this.GetBatchAsync(batch, cancellationToken))
            {
                var packageId = package.Id.ToLowerInvariant();
                var packageVersion = package.NormalizedVersionString.ToLowerInvariant();

                if (!packageDownloads.ContainsKey(packageId) ||
                    !packageDownloads[packageId].ContainsKey(packageVersion))
                {
                    continue;
                }

                package.Downloads = packageDownloads[packageId][packageVersion];
                this._context.Packages.Update(package);
            }

            _ = await this._context.SaveChangesAsync(cancellationToken);

            this._logger.LogInformation("Imported batch {Batch}", batch);
        }
    }

    private Task<List<Package>> GetBatchAsync(int batch, CancellationToken cancellationToken)
    {
        return this._context.Packages
                    .OrderBy(p => p.Key)
                    .Skip(batch * BatchSize)
                    .Take(BatchSize)
                    .ToListAsync(cancellationToken);
    }
}