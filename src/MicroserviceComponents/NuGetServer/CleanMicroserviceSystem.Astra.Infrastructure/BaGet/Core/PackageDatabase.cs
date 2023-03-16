using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core;

public class PackageDatabase : IPackageDatabase
{
    private readonly IContext _context;

    public PackageDatabase(IContext context)
    {
        this._context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<PackageAddResult> AddAsync(Package package, CancellationToken cancellationToken)
    {
        try
        {
            _ = this._context.Packages.Add(package);

            _ = await this._context.SaveChangesAsync(cancellationToken);

            return PackageAddResult.Success;
        }
        catch (DbUpdateException e)
            when (this._context.IsUniqueConstraintViolationException(e))
        {
            return PackageAddResult.PackageAlreadyExists;
        }
    }

    public async Task<bool> ExistsAsync(string id, CancellationToken cancellationToken)
    {
        return await this._context
            .Packages
            .Where(p => p.Id == id)
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(string id, NuGetVersion version, CancellationToken cancellationToken)
    {
        return await this._context
            .Packages
            .Where(p => p.Id == id)
            .Where(p => p.NormalizedVersionString == version.ToNormalizedString())
            .AnyAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Package>> FindAsync(string id, bool includeUnlisted, CancellationToken cancellationToken)
    {
        var query = this._context.Packages
            .Include(p => p.Dependencies)
            .Include(p => p.PackageTypes)
            .Include(p => p.TargetFrameworks)
            .Where(p => p.Id == id);

        if (!includeUnlisted)
            query = query.Where(p => p.Listed);

        return (await query.ToListAsync(cancellationToken)).AsReadOnly();
    }

    public Task<Package> FindOrNullAsync(
        string id,
        NuGetVersion version,
        bool includeUnlisted,
        CancellationToken cancellationToken)
    {
        var query = this._context.Packages
            .Include(p => p.Dependencies)
            .Include(p => p.TargetFrameworks)
            .Where(p => p.Id == id)
            .Where(p => p.NormalizedVersionString == version.ToNormalizedString());

        if (!includeUnlisted)
            query = query.Where(p => p.Listed);

        return query.FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> UnlistPackageAsync(string id, NuGetVersion version, CancellationToken cancellationToken)
    {
        return this.TryUpdatePackageAsync(id, version, p => p.Listed = false, cancellationToken);
    }

    public Task<bool> RelistPackageAsync(string id, NuGetVersion version, CancellationToken cancellationToken)
    {
        return this.TryUpdatePackageAsync(id, version, p => p.Listed = true, cancellationToken);
    }

    public async Task AddDownloadAsync(string id, NuGetVersion version, CancellationToken cancellationToken)
    {
        _ = await this.TryUpdatePackageAsync(id, version, p => p.Downloads += 1, cancellationToken);
    }

    public async Task<bool> HardDeletePackageAsync(string id, NuGetVersion version, CancellationToken cancellationToken)
    {
        var package = await this._context.Packages
            .Where(p => p.Id == id)
            .Where(p => p.NormalizedVersionString == version.ToNormalizedString())
            .Include(p => p.Dependencies)
            .Include(p => p.TargetFrameworks)
            .FirstOrDefaultAsync(cancellationToken);

        if (package == null)
            return false;

        _ = this._context.Packages.Remove(package);
        _ = await this._context.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task<bool> TryUpdatePackageAsync(
        string id,
        NuGetVersion version,
        Action<Package> action,
        CancellationToken cancellationToken)
    {
        var package = await this._context.Packages
            .Where(p => p.Id == id)
            .Where(p => p.NormalizedVersionString == version.ToNormalizedString())
            .FirstOrDefaultAsync();

        if (package != null)
        {
            action(package);
            _ = await this._context.SaveChangesAsync(cancellationToken);

            return true;
        }

        return false;
    }
}