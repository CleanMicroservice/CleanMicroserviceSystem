using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Validation;

public class ValidateStartupOptions
{
    private readonly IOptions<BaGetOptions> _root;
    private readonly IOptions<DatabaseOptions> _database;
    private readonly IOptions<StorageOptions> _storage;
    private readonly IOptions<MirrorOptions> _mirror;
    private readonly ILogger<ValidateStartupOptions> _logger;

    public ValidateStartupOptions(
        IOptions<BaGetOptions> root,
        IOptions<DatabaseOptions> database,
        IOptions<StorageOptions> storage,
        IOptions<MirrorOptions> mirror,
        ILogger<ValidateStartupOptions> logger)
    {
        this._root = root ?? throw new ArgumentNullException(nameof(root));
        this._database = database ?? throw new ArgumentNullException(nameof(database));
        this._storage = storage ?? throw new ArgumentNullException(nameof(storage));
        this._mirror = mirror ?? throw new ArgumentNullException(nameof(mirror));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public bool Validate()
    {
        try
        {
            _ = this._root.Value;
            _ = this._database.Value;
            _ = this._storage.Value;
            _ = this._mirror.Value;

            return true;
        }
        catch (OptionsValidationException e)
        {
            foreach (var failure in e.Failures)
            {
                this._logger.LogError("{OptionsFailure}", failure);
            }

            this._logger.LogError(e, "BaGet configuration is invalid.");
            return false;
        }
    }
}