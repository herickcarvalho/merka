namespace Mercado.BuildingBlocks.Domain.Events;

/// <summary>
/// Marca um evento de domínio (algo relevante que aconteceu dentro de um
/// agregado, ex.: "PedidoCriado", "EstoqueBaixou"). Eventos de domínio
/// permitem que módulos reajam a mudanças de outros módulos sem
/// acoplamento direto (comunicação assíncrona/in-process via MediatR
/// notifications, configurado na camada Application).
/// </summary>
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
