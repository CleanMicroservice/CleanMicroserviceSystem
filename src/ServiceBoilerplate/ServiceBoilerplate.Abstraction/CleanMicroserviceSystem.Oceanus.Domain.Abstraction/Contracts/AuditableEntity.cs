namespace CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Contracts;

public abstract class AuditableEntity<TKey> : Entity<TKey>, IAuditableEntity<TKey>
{
    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }
}
