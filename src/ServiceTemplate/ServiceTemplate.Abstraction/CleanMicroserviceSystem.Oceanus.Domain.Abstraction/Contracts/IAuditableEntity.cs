namespace CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Contracts;

public interface IAuditableEntity<TKey> : IEntity<TKey>
{
    int CreatedBy { get; set; }

    DateTime CreatedOn { get; set; }

    int? LastModifiedBy { get; set; }

    DateTime? LastModifiedOn { get; set; }
}
