namespace Mercado.BuildingBlocks.Application.Abstractions;

/// <summary>
/// Abstração de Unit of Work: permite à camada Application persistir
/// alterações sem conhecer EF Core. A implementação concreta vive na
/// Infrastructure de cada módulo (encapsulando o respectivo DbContext).
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
