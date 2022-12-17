namespace CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Contracts;

public interface IAuditableEntity<TID> : IAuditableEntity, IEntity<TID>
{
}

public interface IAuditableEntity : IEntity
{
    int CreatedBy { get; set; }

    DateTime CreatedOn { get; set; }

    int? LastModifiedBy { get; set; }

    DateTime? LastModifiedOn { get; set; }
}
