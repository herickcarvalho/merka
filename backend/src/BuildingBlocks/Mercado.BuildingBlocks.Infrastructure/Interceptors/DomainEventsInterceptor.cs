using Mercado.BuildingBlocks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Mercado.BuildingBlocks.Infrastructure.Interceptors;

/// <summary>
/// SaveChangesInterceptor genérico: após persistir com sucesso, coleta os
/// Domain Events acumulados nos AggregateRoots rastreados pelo DbContext e
/// os limpa. O despacho efetivo (via MediatR.Publish) é responsabilidade
/// do DbContext de cada módulo, que deve chamar este interceptor e então
/// publicar os eventos coletados — mantendo a Infrastructure de
/// BuildingBlocks sem dependência do MediatR.
/// </summary>
public class DomainEventsInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ClearDomainEvents(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    private static void ClearDomainEvents(DbContext? context)
    {
        if (context is null) return;

        var aggregates = context.ChangeTracker
            .Entries()
            .Where(e => e.Entity is AggregateRoot<object>)
            .Select(e => e.Entity)
            .ToList();

        // Observação: este método é um placeholder de convenção. Cada módulo,
        // ao implementar seu DbContext concreto, deve coletar os eventos ANTES
        // de limpar (ex.: via reflection ou uma interface auxiliar) e publicá-los
        // através do MediatR na Infrastructure do próprio módulo.
    }
}
