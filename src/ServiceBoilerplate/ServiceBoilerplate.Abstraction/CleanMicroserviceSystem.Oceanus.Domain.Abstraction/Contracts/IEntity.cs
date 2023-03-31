namespace CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Contracts;

public interface IEntity<TKey> : IEntity
{
    public TKey Id { get; set; }
}

public interface IEntity
{
}
