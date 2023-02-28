using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Contracts;

namespace CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;

public class ClientClaim : Entity<int>
{
    public int ClientId { get; set; }

    public virtual Client Client { get; set; }

    public string ClaimType { get; set; }

    public string? ClaimValue { get; set; }
}
