using Microsoft.Extensions.DependencyInjection;

namespace Mercado.Modules.Inventory.Infrastructure.DependencyInjection;

/// <summary>
/// Ponto único de registro dos serviços de Infrastructure do módulo
/// Inventory (DbContext, repositórios). Chamado pelo host (Mercado.Api).
/// Corpo intencionalmente mínimo: nenhum DbContext/entidade foi criado
/// ainda para este módulo.
/// </summary>
public static class InventoryInfrastructureModule
{
    public static IServiceCollection AddInventoryInfrastructure(this IServiceCollection services, string connectionString)
    {
        // Exemplo futuro:
        // services.AddDbContext<InventoryDbContext>(opt => opt.UseNpgsql(connectionString));
        // services.AddScoped<IInventoryRepository, InventoryRepository>();
        return services;
    }
}
