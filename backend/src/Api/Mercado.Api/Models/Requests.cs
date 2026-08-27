
namespace Mercado.Api.Models;

public record CategoryRequest(string Name);
public record BrandRequest(string Name);
public record ProductRequest(
    string Name,
    string? Description,
    string Sku,
    string? Barcode,
    Guid CategoryId,
    Guid? BrandId,
    decimal CostPrice,
    decimal SalePrice,
    int MinimumStock,
    decimal InitialStock = 0);

public record SupplierRequest(string Name, string? Document, string? Phone);
public record CustomerRequest(string Name, string? Document, string? Phone);
public record TransactionItemRequest(Guid ProductId, decimal Quantity, decimal UnitPrice);
public record PurchaseRequest(Guid SupplierId, List<TransactionItemRequest> Items);
public record SaleRequest(Guid? CustomerId, string PaymentMethod, List<TransactionItemRequest> Items);
