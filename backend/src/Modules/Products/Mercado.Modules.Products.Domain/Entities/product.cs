

namespace Mercado.Modules.Products.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }

    public string Name { get; private set;} 

    public string Description { get; private set; }

    public string SKU {get; private set;  }

    public string? Barcode { get; private set;}

    public Guid CategoryId { get; private set;}

    public Guid BrandId { get; private set;}

    public decimal CostPrice {get; private set;}

    public decimal SalePrice {get; private set;}

    public int MinimumStock {get; private set;}

    public bool IsActive {get; private set;}

    public DateTime CreatedAt {get; private set;}

    public DateTime? UpdatedAt {get; private set;}



    public Product (
        string name,
        string description,
        string sku,
        Guid categoryId,
        Guid brandId,
        decimal costPrice,
        decimal salePrice,
        string? barcode,
        int minimumStock
        )

        private void ValidateName(Name)
{
    if (string.IsNullOrWhiteSpace(name))
    {
        throw new ProductDomainException("O nome do produto é obrigatório.");
    } 
}

ValidateName(name);
        
        {

            Id = Guid.NewGuid();


            Name = name;
         
            Description = description;
            SKU = sku;
            CategoryId = categoryId;
            BrandId = brandId;
            CostPrice = costPrice;
            SalePrice = salePrice;
            Barcode = barcode;
            MinimumStock = minimumStock;
            
            IsActive = true;  
            CreatedAt = DateTime.UtcNow;
    }


}




