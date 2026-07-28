using Mercado.Modules.Products.Domain.Exceptions;

namespace Mercado.Modules.Products.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public string SKU { get; private set; }

    public string? Barcode { get; private set; }

    public Guid CategoryId { get; private set; }

    public Guid? BrandId { get; private set; }

    public decimal CostPrice { get; private set; }

    public decimal SalePrice { get; private set; }

    public int MinimumStock { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public Product(
        string name,
        string description,
        string sku,
        Guid categoryId,
        Guid? brandId,
        decimal costPrice,
        decimal salePrice,
        string? barcode,
        int minimumStock)
    {
        ValidateName(name);

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


          public void ChangeMinimumStock(int newMinimumStock)
        {
    if (newMinimumStock < 0)
        }
    
        throw new ProductDomainException("...");
    }

       MinimumStock=newMinimumStock;
      
            }    

            public void changeName(string newName)
            {
                if(string.IsNullOrWhiteSpace(newName) )
                {
                throw new ProductDomainException("O nome do produto é obrigatório");
                }

                if(newName.Length < 3 )
                 {

               throw new ProductDomainException("O produto deve possuir pelo menos 3 caracteres");
               }

               if (Name == newName)
                  {
                throw new ProductDomainException("O novo nome deve ser diferente do nome atual.");
                   }
            }

          
            Name = newName;
               
               }

    private void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ProductDomainException(
                "O nome do produto é obrigatório.");
        }
    }
}

public void Deactivate()
{
    isActive = false;
}



