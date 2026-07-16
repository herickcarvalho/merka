using Microsoft.Extensions.DependencyInjection;

namespace Mercado.Modules.Purchasing.Infrastructure.DependencyInjection;

/// <summary>
/// Ponto único de registro dos serviços de Infrastructure do módulo
/// Purchasing (DbContext, repositórios). Chamado pelo host (Mercado.Api).
/// Corpo intencionalmente mínimo: nenhum DbContext/entidade foi criado
/// ainda para este módulo.
/// </summary>
public static class PurchasingInfrastructureModule
{
    public static IServiceCollection AddPurchasingInfrastructure(this IServiceCollection services, string connectionString)
    {
        // Exemplo futuro:
        // services.AddDbContext<PurchasingDbContext>(opt => opt.UseNpgsql(connectionString));
        // services.AddScoped<IPurchasingRepository, PurchasingRepository>();
        return services;
    }
}
