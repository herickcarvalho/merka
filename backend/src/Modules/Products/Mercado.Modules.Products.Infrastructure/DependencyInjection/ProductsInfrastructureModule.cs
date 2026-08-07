using Microsoft.Extensions.DependencyInjection;

namespace Mercado.Modules.Products.Infrastructure.DependencyInjection;

/// <summary>
/// Ponto único de registro dos serviços de Infrastructure do módulo
/// Products (DbContext, repositórios). Chamado pelo host (Mercado.Api).
/// Corpo intencionalmente mínimo: nenhum DbContext/entidade foi criado
/// ainda para este módulo.
/// </summary>
public static class ProductsInfrastructureModule
{
    public static IServiceCollection AddProductsInfrastructure(this IServiceCollection services, string connectionString)
    {
        // Exemplo futuro:
        // services.AddDbContext<ProductsDbContext>(opt => opt.UseNpgsql(connectionString));
        // services.AddScoped<IProductsRepository, ProductsRepository>();
        return services;
    }
}
