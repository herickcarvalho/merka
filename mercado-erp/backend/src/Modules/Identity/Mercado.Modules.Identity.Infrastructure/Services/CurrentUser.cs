using Mercado.BuildingBlocks.Application.Abstractions;

namespace Mercado.Modules.Identity.Infrastructure.Services;

public class CurrentUser : ICurrentUser
{
    public UserRole Role => UserRole.Manager;
}
}