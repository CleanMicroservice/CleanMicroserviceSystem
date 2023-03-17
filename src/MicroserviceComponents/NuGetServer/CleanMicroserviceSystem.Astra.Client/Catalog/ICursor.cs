namespace CleanMicroserviceSystem.Astra.Client.Catalog;

public interface ICursor
{
    Task<DateTimeOffset?> GetAsync(CancellationToken cancellationToken = default);

    Task SetAsync(DateTimeOffset value, CancellationToken cancellationToken = default);
}