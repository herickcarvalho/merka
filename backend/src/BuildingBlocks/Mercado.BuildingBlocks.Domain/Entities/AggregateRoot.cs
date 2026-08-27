using Mercado.BuildingBlocks.Domain.Events;

namespace Mercado.BuildingBlocks.Domain.Entities;

/// <summary>
/// Um Aggregate Root é a "porta de entrada" de um agregado DDD: todas as
/// alterações a entidades internas do agregado devem passar por ele. Também
/// é responsável por acumular Domain Events que serão despachados após a
/// persistência (ver Mercado.BuildingBlocks.Infrastructure).
/// </summary>
public abstract class AggregateRoot<TId> : BaseEntity<TId> where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected AggregateRoot() { }

    protected AggregateRoot(TId id) : base(id) { }

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}
