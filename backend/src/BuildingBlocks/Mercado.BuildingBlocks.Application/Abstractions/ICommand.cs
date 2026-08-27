using MediatR;

namespace Mercado.BuildingBlocks.Application.Abstractions;

/// <summary>
/// Marca um Command no padrão CQRS: uma intenção de ALTERAR estado do
/// sistema (ex.: CriarProdutoCommand, RegistrarVendaCommand). Cada módulo
/// implementa seus próprios commands/handlers dentro de UseCases/.
/// </summary>
public interface ICommand : IRequest<Unit> { }

/// <summary>Command que retorna um valor após a alteração (ex.: o Id criado).</summary>
public interface ICommand<out TResponse> : IRequest<TResponse> { }
