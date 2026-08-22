using Mercado.BuildingBlocks.Application.Abstractions;
using Mercado.Modules.Identity.Infrastructure.Services;
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
      services.AddScoped<ICurrentUser, CurrentUser>();
        return services;
    }
}
