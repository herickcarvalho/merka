
using Mercado.BuildingBlocks.Application.Abstractions;
namespace Mercado.Modules.Products.Application.UseCases.ChangeSalePrice;

public class ChangeSalePriceUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUser _currentUser;

    public ChangeSalePriceUseCase(
    IProductRepository productRepository,
    ICurrentUser currentUser)
    {
        _productRepository = productRepository;
         _currentUser = currentUser;
    }

public void Execute(Guid productId, decimal newSalePrice)
{
    if (_currentUser.Role != UserRole.Manager)
    {
        throw new ProductDomainException("Usuário não autorizado a alterar o preço.");
    }

    var product = _productRepository.GetById(productId);

    if (product == null)
    {
        throw new ProductDomainException("Produto não encontrado.");
    }

    product.ChangeSalePrice(newSalePrice);

    _productRepository.Update(product);
}