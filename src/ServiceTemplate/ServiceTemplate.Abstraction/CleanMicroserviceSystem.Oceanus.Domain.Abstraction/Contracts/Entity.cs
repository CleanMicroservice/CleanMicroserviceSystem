namespace CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Contracts;

public class Entity<TKey> : Entity, IEntity<TKey>
{
    public TKey Id { get; set; }
}

public class Entity : IEntity
{
}
