using Microsoft.Extensions.DependencyInjection;

namespace Mercado.Modules.Products.Application.DependencyInjection;

/// <summary>
/// Ponto único de registro dos serviços da camada Application do módulo
/// Products. O host (Mercado.Api) chama este método sem precisar conhecer
/// detalhes internos do módulo — apenas AddProductsApplication().
/// Corpo intencionalmente mínimo: nenhum caso de uso foi implementado
/// ainda, então não há Handlers/Validators para registrar.
/// </summary>
public static class ProductsApplicationModule
{
    public static IServiceCollection AddProductsApplication(this IServiceCollection services)
    {
        // Exemplo futuro:
        // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ProductsApplicationModule).Assembly));
        // services.AddValidatorsFromAssembly(typeof(ProductsApplicationModule).Assembly);
        return services;
    }
}
