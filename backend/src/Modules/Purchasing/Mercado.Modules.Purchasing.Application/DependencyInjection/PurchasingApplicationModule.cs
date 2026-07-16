using Microsoft.Extensions.DependencyInjection;

namespace Mercado.Modules.Purchasing.Application.DependencyInjection;

/// <summary>
/// Ponto único de registro dos serviços da camada Application do módulo
/// Purchasing. O host (Mercado.Api) chama este método sem precisar conhecer
/// detalhes internos do módulo — apenas AddPurchasingApplication().
/// Corpo intencionalmente mínimo: nenhum caso de uso foi implementado
/// ainda, então não há Handlers/Validators para registrar.
/// </summary>
public static class PurchasingApplicationModule
{
    public static IServiceCollection AddPurchasingApplication(this IServiceCollection services)
    {
        // Exemplo futuro:
        // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(PurchasingApplicationModule).Assembly));
        // services.AddValidatorsFromAssembly(typeof(PurchasingApplicationModule).Assembly);
        return services;
    }
}
