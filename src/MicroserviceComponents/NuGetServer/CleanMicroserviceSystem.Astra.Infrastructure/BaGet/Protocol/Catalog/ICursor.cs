namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Catalog;

public interface ICursor
{
    Task<DateTimeOffset?> GetAsync(CancellationToken cancellationToken = default);

    Task SetAsync(DateTimeOffset value, CancellationToken cancellationToken = default);
}