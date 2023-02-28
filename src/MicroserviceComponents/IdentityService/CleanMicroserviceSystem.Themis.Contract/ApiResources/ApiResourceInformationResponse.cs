using CleanMicroserviceSystem.Oceanus.Contract.Abstraction;

namespace CleanMicroserviceSystem.Themis.Contract.ApiResources;

public class ApiResourceInformationResponse : ContractBase
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool Enabled { get; set; }

    public string? Description { get; set; }
}
