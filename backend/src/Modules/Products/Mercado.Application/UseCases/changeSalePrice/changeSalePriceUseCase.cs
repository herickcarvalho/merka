using Mercado.Modules.Products.Domain.Repositories;

namespace Mercado.Modules.Products.Application.UseCases.ChangeSalePrice;

public class ChangeSalePriceUseCase
{
    private readonly IProductRepository _productRepository;

    public ChangeSalePriceUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public void Execute(Guid productId, decimal newSalePrice)
    {
        var product = _productRepository.GetById(productId)
            ?? throw new InvalidOperationException("Produto não encontrado.");

        product.ChangeSalePrice(newSalePrice);
        _productRepository.Update(product);
    }
}
