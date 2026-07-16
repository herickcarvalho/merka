namespace Mercado.BuildingBlocks.Domain.Common;

/// <summary>
/// Base para "enums ricos" (padrão Enumeration), útil quando um enum comum
/// do C# não é suficiente (ex.: quando cada valor precisa de comportamento
/// ou metadados associados). Uso opcional pelos módulos de negócio.
/// </summary>
public abstract class Enumeration : IComparable
{
    public string Name { get; }
    public int Id { get; }

    protected Enumeration(int id, string name) => (Id, Name) = (id, name);

    public override string ToString() => Name;

    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration other) return false;
        return GetType() == other.GetType() && Id == other.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public int CompareTo(object? obj) => Id.CompareTo(((Enumeration)obj!).Id);
}
