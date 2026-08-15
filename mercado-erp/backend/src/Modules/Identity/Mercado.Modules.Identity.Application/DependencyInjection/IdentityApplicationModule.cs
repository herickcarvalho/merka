using Microsoft.Extensions.DependencyInjection;

namespace Mercado.Modules.Identity.Application.DependencyInjection;

/// <summary>
/// Ponto único de registro dos serviços da camada Application do módulo
/// Identity. O host (Mercado.Api) chama este método sem precisar conhecer
/// detalhes internos do módulo — apenas AddIdentityApplication().
/// Corpo intencionalmente mínimo: nenhum caso de uso foi implementado
/// ainda, então não há Handlers/Validators para registrar.
/// </summary>
public static class IdentityApplicationModule
{
    public static IServiceCollection AddIdentityApplication(this IServiceCollection services)
    {
        // Exemplo futuro:
        // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IdentityApplicationModule).Assembly));
        // services.AddValidatorsFromAssembly(typeof(IdentityApplicationModule).Assembly);
        return services;
    }
}
