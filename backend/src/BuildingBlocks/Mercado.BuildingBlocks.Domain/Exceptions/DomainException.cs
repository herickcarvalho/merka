namespace Mercado.BuildingBlocks.Domain.Exceptions;

/// <summary>
/// Exceção-base para violações de regras de negócio dentro do domínio.
/// Módulos específicos devem criar exceções que herdam desta (ex.:
/// EstoqueInsuficienteException) em vez de lançar Exception genérica.
/// </summary>
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
}
