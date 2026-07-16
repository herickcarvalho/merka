using Microsoft.Extensions.DependencyInjection;

namespace Mercado.Modules.Inventory.Application.DependencyInjection;

/// <summary>
/// Ponto único de registro dos serviços da camada Application do módulo
/// Inventory. O host (Mercado.Api) chama este método sem precisar conhecer
/// detalhes internos do módulo — apenas AddInventoryApplication().
/// Corpo intencionalmente mínimo: nenhum caso de uso foi implementado
/// ainda, então não há Handlers/Validators para registrar.
/// </summary>
public static class InventoryApplicationModule
{
    public static IServiceCollection AddInventoryApplication(this IServiceCollection services)
    {
        // Exemplo futuro:
        // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(InventoryApplicationModule).Assembly));
        // services.AddValidatorsFromAssembly(typeof(InventoryApplicationModule).Assembly);
        return services;
    }
}
