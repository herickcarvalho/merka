using Mercado.BuildingBlocks.Application.Abstractions;

namespace Mercado.Modules.Identity.Infrastructure.Services;

public class CurrentUser : ICurrentUser
{
    public CurrentUser(UserRole role)
    {
        Role = role;
    }

    public UserRole Role { get; }
}