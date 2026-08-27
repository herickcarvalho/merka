namespace Mercado.BuildingBlocks.Domain.ValueObjects;

/// <summary>
/// Classe-base para Value Objects (padrão DDD): objetos sem identidade
/// própria, cuja igualdade é definida pelo valor de seus componentes
/// (ex.: Dinheiro, Endereco, CPF/CNPJ). Módulos de negócio devem herdar
/// desta classe e implementar <see cref="GetEqualityComponents"/>.
/// </summary>
public abstract class ValueObject
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is not ValueObject other || GetType() != other.GetType())
            return false;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode() =>
        GetEqualityComponents().Aggregate(1, (current, obj) =>
            HashCode.Combine(current, obj?.GetHashCode() ?? 0));

    public static bool operator ==(ValueObject? left, ValueObject? right) => Equals(left, right);

    public static bool operator !=(ValueObject? left, ValueObject? right) => !Equals(left, right);
}
