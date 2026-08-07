using Microsoft.Extensions.DependencyInjection;

namespace Mercado.Modules.Sales.Application.DependencyInjection;

/// <summary>
/// Ponto único de registro dos serviços da camada Application do módulo
/// Sales. O host (Mercado.Api) chama este método sem precisar conhecer
/// detalhes internos do módulo — apenas AddSalesApplication().
/// Corpo intencionalmente mínimo: nenhum caso de uso foi implementado
/// ainda, então não há Handlers/Validators para registrar.
/// </summary>
public static class SalesApplicationModule
{
    public static IServiceCollection AddSalesApplication(this IServiceCollection services)
    {
        // Exemplo futuro:
        // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SalesApplicationModule).Assembly));
        // services.AddValidatorsFromAssembly(typeof(SalesApplicationModule).Assembly);
        return services;
    }
}
