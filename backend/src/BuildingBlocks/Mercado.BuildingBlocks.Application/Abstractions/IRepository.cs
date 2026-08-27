using Mercado.BuildingBlocks.Domain.Entities;

namespace Mercado.BuildingBlocks.Application.Abstractions;

/// <summary>
/// Contrato genérico de repositório. Cada módulo cria repositórios
/// específicos (ex.: IProdutoRepository : IRepository&lt;Produto, Guid&gt;)
/// adicionando métodos de consulta próprios do seu domínio.
/// </summary>
public interface IRepository<TEntity, in TId> where TEntity : BaseEntity<TId> where TId : notnull
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}
