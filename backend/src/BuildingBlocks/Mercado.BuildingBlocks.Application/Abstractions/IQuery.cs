using MediatR;

namespace Mercado.BuildingBlocks.Application.Abstractions;

/// <summary>
/// Marca uma Query no padrão CQRS: uma LEITURA que não altera estado
/// (ex.: ObterProdutoPorIdQuery, ListarProdutosQuery).
/// </summary>
public interface IQuery<out TResponse> : IRequest<TResponse> { }
