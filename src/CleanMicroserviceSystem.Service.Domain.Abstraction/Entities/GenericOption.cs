using CleanMicroserviceSystem.Service.Domain.Abstraction.Contracts;

namespace CleanMicroserviceSystem.Service.Domain.Abstraction.Entities;

public class GenericOption : AuditableEntity<int>
{
    public string OptionName { get; set; }

    public string? OptionValue { get; set; }

    public string? Category { get; set; }

    public string? OwnerLevel { get; set; }
}
