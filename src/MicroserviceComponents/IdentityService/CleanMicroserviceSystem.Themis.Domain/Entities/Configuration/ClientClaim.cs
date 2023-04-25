using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Contracts;

namespace CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;

public class ClientClaim : Entity<int>
{
    public int ClientId { get; set; }

    public virtual Client Client { get; set; } = default!;

    public string ClaimType { get; set; } = default!;

    public string? ClaimValue { get; set; }
}
