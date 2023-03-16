using System.Reflection.Metadata;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Extensions;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Storage;
using Microsoft.Extensions.Logging;
using NuGet.Packaging;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Indexing;

public class SymbolIndexingService : ISymbolIndexingService
{
    private static readonly HashSet<string> ValidSymbolPackageContentExtensions = new()
    {
        ".pdb",
        ".nuspec",
        ".xml",
        ".psmdcp",
        ".rels",
        ".p7s"
    };

    private readonly IPackageDatabase _packages;
    private readonly ISymbolStorageService _storage;
    private readonly ILogger<SymbolIndexingService> _logger;

    public SymbolIndexingService(
        IPackageDatabase packages,
        ISymbolStorageService storage,
        ILogger<SymbolIndexingService> logger)
    {
        this._packages = packages ?? throw new ArgumentNullException(nameof(packages));
        this._storage = storage ?? throw new ArgumentNullException(nameof(storage));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<SymbolIndexingResult> IndexAsync(Stream stream, CancellationToken cancellationToken)
    {
        try
        {
            using var symbolPackage = new PackageArchiveReader(stream, leaveStreamOpen: true);
            var pdbPaths = await this.GetSymbolPackagePdbPathsOrNullAsync(symbolPackage, cancellationToken);
            if (pdbPaths == null)
                return SymbolIndexingResult.InvalidSymbolPackage;

            var packageId = symbolPackage.NuspecReader.GetId();
            var packageVersion = symbolPackage.NuspecReader.GetVersion();

            var package = await this._packages.FindOrNullAsync(packageId, packageVersion, includeUnlisted: true, cancellationToken);
            if (package == null)
                return SymbolIndexingResult.PackageNotFound;

            using var pdbs = new PdbList();
            foreach (var pdbPath in pdbPaths)
            {
                var portablePdb = await this.ExtractPortablePdbAsync(symbolPackage, pdbPath, cancellationToken);
                if (portablePdb == null)
                    return SymbolIndexingResult.InvalidSymbolPackage;

                pdbs.Add(portablePdb);
            }

            foreach (var pdb in pdbs)
            {
                await this._storage.SavePortablePdbContentAsync(pdb.Filename, pdb.Key, pdb.Content, cancellationToken);
            }

            return SymbolIndexingResult.Success;
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Unable to index symbol package due to exception");
            return SymbolIndexingResult.InvalidSymbolPackage;
        }
    }

    private async Task<IReadOnlyList<string>> GetSymbolPackagePdbPathsOrNullAsync(
        PackageArchiveReader symbolPackage,
        CancellationToken cancellationToken)
    {
        try
        {
            await symbolPackage.ValidatePackageEntriesAsync(cancellationToken);

            var files = (await symbolPackage.GetFilesAsync(cancellationToken)).ToList();

            return !this.AreSymbolFilesValid(files) ? null : (IReadOnlyList<string>)files.Where(p => Path.GetExtension(p) == ".pdb").ToList();
        }
        catch (Exception)
        {
            return null;
        }
    }

    private bool AreSymbolFilesValid(IReadOnlyList<string> entries)
    {
        static bool IsValidSymbolFileInfo(FileInfo file)
        {
            if (string.IsNullOrEmpty(file.Name)) return false;
            if (string.IsNullOrEmpty(file.Extension)) return false;
            return ValidSymbolPackageContentExtensions.Contains(file.Extension);
        }

        return entries.Select(e => new FileInfo(e)).All(IsValidSymbolFileInfo);
    }

    private async Task<PortablePdb> ExtractPortablePdbAsync(
        PackageArchiveReader symbolPackage,
        string pdbPath,
        CancellationToken cancellationToken)
    {
        Stream pdbStream = null;
        PortablePdb result = null;

        try
        {
            using var rawPdbStream = await symbolPackage.GetStreamAsync(pdbPath, cancellationToken);
            pdbStream = await rawPdbStream.AsTemporaryFileStreamAsync();

            string signature;
            using (var pdbReaderProvider = MetadataReaderProvider.FromPortablePdbStream(pdbStream, MetadataStreamOptions.LeaveOpen))
            {
                var reader = pdbReaderProvider.GetMetadataReader();
                var id = new BlobContentId(reader.DebugMetadataHeader.Id);

                signature = id.Guid.ToString("N").ToUpperInvariant();
            }

            var fileName = Path.GetFileName(pdbPath).ToLowerInvariant();
            var key = $"{signature}ffffffff";

            pdbStream.Position = 0;
            result = new PortablePdb(fileName, key, pdbStream);
        }
        finally
        {
            if (result == null)
                pdbStream?.Dispose();
        }

        return result;
    }

    private class PortablePdb : IDisposable
    {
        public PortablePdb(string filename, string key, Stream content)
        {
            this.Filename = filename;
            this.Key = key;
            this.Content = content;
        }

        public string Filename { get; }
        public string Key { get; }
        public Stream Content { get; }

        public void Dispose()
        {
            this.Content.Dispose();
        }
    }

    private class PdbList : List<PortablePdb>, IDisposable
    {
        public void Dispose()
        {
            foreach (var pdb in this)
            {
                pdb.Dispose();
            }
        }
    }
}