using CleanMicroserviceSystem.Oceanus.Contract.Abstraction;

namespace CleanMicroserviceSystem.Themis.Contract.Clients;

public class ClientUpdateRequest : ContractBase
{
    public string? Name { get; set; }

    public bool? Enabled { get; set; }

    public string? Description { get; set; }

    public string? Secret { get; set; }
}
