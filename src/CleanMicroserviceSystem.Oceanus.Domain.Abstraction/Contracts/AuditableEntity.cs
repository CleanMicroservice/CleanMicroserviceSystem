namespace CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Contracts;

public abstract class AuditableEntity<TID> : IAuditableEntity<TID>
{
    public TID ID { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }
}
