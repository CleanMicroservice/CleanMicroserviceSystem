using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Contracts;

namespace CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;

public class GenericOption : AuditableEntity<int>
{
    public string OptionName { get; set; } = default!;

    public string? OptionValue { get; set; }

    public string? Category { get; set; }

    public string? OwnerLevel { get; set; }
}
