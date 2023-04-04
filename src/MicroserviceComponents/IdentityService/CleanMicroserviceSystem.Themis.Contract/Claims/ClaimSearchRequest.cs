using CleanMicroserviceSystem.Oceanus.Contract.Abstraction;

namespace CleanMicroserviceSystem.Themis.Contract.Claims;

public class ClaimSearchRequest : ContractBase
{
    public int? UserId { get; set; } = default;

    public string? Type { get; set; } = default;

    public string? Value { get; set; } = default;

    public int? Start { get; set; }

    public int? Count { get; set; }
}
