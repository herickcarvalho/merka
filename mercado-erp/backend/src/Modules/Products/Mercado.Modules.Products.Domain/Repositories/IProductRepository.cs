

namespace Mercado.Modules.Products.Domain.Repositories;

public interface IProductRepository
{
    Product? GetById(Guid productId);

    void Update(Product product);
}