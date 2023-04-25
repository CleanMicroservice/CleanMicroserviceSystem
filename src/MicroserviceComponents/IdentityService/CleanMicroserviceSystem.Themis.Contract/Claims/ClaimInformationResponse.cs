using CleanMicroserviceSystem.Oceanus.Contract.Abstraction;

namespace CleanMicroserviceSystem.Themis.Contract.Claims;

public class ClaimInformationResponse : ContractBase
{
    public string Type { get; set; } = default!;

    public string? Value { get; set; }
}
