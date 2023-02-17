namespace CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Contracts;

public abstract class AuditableEntity<TID> : AuditableEntity, IAuditableEntity<TID>
{
    public TID ID { get; set; }

}

public abstract class AuditableEntity : IAuditableEntity
{
    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }
}
