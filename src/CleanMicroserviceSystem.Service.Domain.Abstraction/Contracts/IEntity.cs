namespace CleanMicroserviceSystem.Service.Domain.Abstraction.Contracts;

public interface IEntity<TID> : IEntity
{
    public TID ID { get; set; }
}

public interface IEntity
{
}
