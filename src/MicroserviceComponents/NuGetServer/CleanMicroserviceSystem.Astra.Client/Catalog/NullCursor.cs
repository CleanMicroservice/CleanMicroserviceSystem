namespace CleanMicroserviceSystem.Astra.Client.Catalog;

public class NullCursor : ICursor
{
    public Task<DateTimeOffset?> GetAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<DateTimeOffset?>(null);
    }

    public Task SetAsync(DateTimeOffset value, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}