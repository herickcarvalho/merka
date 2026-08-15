using Microsoft.Extensions.DependencyInjection;

namespace Mercado.Modules.Identity.Infrastructure.DependencyInjection;

/// <summary>
/// Ponto único de registro dos serviços de Infrastructure do módulo
/// Identity (DbContext, repositórios). Chamado pelo host (Mercado.Api).
/// Corpo intencionalmente mínimo: nenhum DbContext/entidade foi criado
/// ainda para este módulo.
/// </summary>
public static class IdentityInfrastructureModule
{
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, string connectionString)
    {
        // Exemplo futuro:
        // services.AddDbContext<IdentityDbContext>(opt => opt.UseNpgsql(connectionString));
        // services.AddScoped<IIdentityRepository, IdentityRepository>();
        return services;
    }
}
