namespace Mercado.BuildingBlocks.Application.Abstractions;

public interface ICurrentUser
{
    UserRole Role { get; }
}