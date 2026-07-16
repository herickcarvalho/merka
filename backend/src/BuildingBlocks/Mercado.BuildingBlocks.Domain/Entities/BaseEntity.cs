namespace Mercado.BuildingBlocks.Domain.Entities;

/// <summary>
/// Classe-base para entidades com identidade (padrão DDD). Toda entidade de
/// qualquer módulo de negócio deve herdar desta classe (diretamente ou via
/// <see cref="AggregateRoot{TId}"/>) para garantir consistência de igualdade
/// baseada em identidade, e não em valor dos campos.
/// </summary>
public abstract class BaseEntity<TId> where TId : notnull
{
    public TId Id { get; protected set; } = default!;

    protected BaseEntity() { }

    protected BaseEntity(TId id) => Id = id;

    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity<TId> other) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;

        return Id.Equals(other.Id);
    }

    public override int GetHashCode() => Id.GetHashCode();
}
