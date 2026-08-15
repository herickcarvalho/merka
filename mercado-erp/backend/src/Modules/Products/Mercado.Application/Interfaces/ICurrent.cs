namespace Mercado.Modules.Products.Application.Interfaces;

public interface ICurrentUser
{
    UserRole Role { get; }
}