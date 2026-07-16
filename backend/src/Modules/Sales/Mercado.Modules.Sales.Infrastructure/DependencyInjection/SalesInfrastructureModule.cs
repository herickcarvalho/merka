using Microsoft.Extensions.DependencyInjection;

namespace Mercado.Modules.Sales.Infrastructure.DependencyInjection;

/// <summary>
/// Ponto único de registro dos serviços de Infrastructure do módulo
/// Sales (DbContext, repositórios). Chamado pelo host (Mercado.Api).
/// Corpo intencionalmente mínimo: nenhum DbContext/entidade foi criado
/// ainda para este módulo.
/// </summary>
public static class SalesInfrastructureModule
{
    public static IServiceCollection AddSalesInfrastructure(this IServiceCollection services, string connectionString)
    {
        // Exemplo futuro:
        // services.AddDbContext<SalesDbContext>(opt => opt.UseNpgsql(connectionString));
        // services.AddScoped<ISalesRepository, SalesRepository>();
        return services;
    }
}
