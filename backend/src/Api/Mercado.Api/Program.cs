
using Mercado.Api.Data;
using Mercado.Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não configurada.");

builder.Services.AddDbContext<MercadoDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks().AddNpgSql(connectionString, name: "postgres");
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy => policy
        .WithOrigins(
            "https://merka-frontend.onrender.com", 
            "http://localhost:5173"
        )
        .AllowAnyHeader()
        .AllowAnyMethod());
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MercadoDbContext>();
    await db.Database.EnsureCreatedAsync();

    if (!await db.Categories.AnyAsync())
    {
        db.Categories.AddRange(
            new Category { Name = "Bebidas" },
            new Category { Name = "Mercearia" },
            new Category { Name = "Limpeza" },
            new Category { Name = "Higiene" });
        await db.SaveChangesAsync();
    }
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("Frontend");
app.MapHealthChecks("/health");

var api = app.MapGroup("/api");

// Dashboard
api.MapGet("/dashboard", async (MercadoDbContext db) =>
{
    var today = DateTime.UtcNow.Date;
    var salesToday = await db.Sales.Where(x => x.CreatedAt >= today).SumAsync(x => (decimal?)x.Total) ?? 0;
    var totalProducts = await db.Products.CountAsync(x => x.IsActive);
    var lowStock = await db.Products.CountAsync(x => x.Stock != null && x.Stock.Quantity <= x.MinimumStock);
    var recentSales = await db.Sales
        .OrderByDescending(x => x.CreatedAt)
        .Take(8)
        .Select(x => new { x.Id, x.Total, x.PaymentMethod, x.CreatedAt })
        .ToListAsync();

    return Results.Ok(new { salesToday, totalProducts, lowStock, recentSales });
});

// Categories
var categories = api.MapGroup("/categories");
categories.MapGet("", async (MercadoDbContext db) =>
    Results.Ok(await db.Categories.OrderBy(x => x.Name).ToListAsync()));
categories.MapPost("", async (CategoryRequest request, MercadoDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(request.Name)) return Results.BadRequest("Nome da categoria é obrigatório.");
    if (await db.Categories.AnyAsync(x => x.Name.ToLower() == request.Name.Trim().ToLower()))
        return Results.Conflict("Já existe uma categoria com esse nome.");

    var category = new Category { Name = request.Name.Trim() };
    db.Categories.Add(category);
    await db.SaveChangesAsync();
    return Results.Created($"/api/categories/{category.Id}", category);
});
categories.MapPut("/{id:guid}", async (Guid id, CategoryRequest request, MercadoDbContext db) =>
{
    var category = await db.Categories.FindAsync(id);
    if (category is null) return Results.NotFound();
    category.Name = request.Name.Trim();
    await db.SaveChangesAsync();
    return Results.Ok(category);
});

// Brands
var brands = api.MapGroup("/brands");
brands.MapGet("", async (MercadoDbContext db) =>
    Results.Ok(await db.Brands.OrderBy(x => x.Name).ToListAsync()));
brands.MapPost("", async (BrandRequest request, MercadoDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(request.Name)) return Results.BadRequest("Nome da marca é obrigatório.");
    var brand = new Brand { Name = request.Name.Trim() };
    db.Brands.Add(brand);
    await db.SaveChangesAsync();
    return Results.Created($"/api/brands/{brand.Id}", brand);
});

// Products
var products = api.MapGroup("/products");
products.MapGet("", async (string? search, MercadoDbContext db) =>
{
    var query = db.Products.Include(x => x.Category).Include(x => x.Brand).Include(x => x.Stock).AsQueryable();
    if (!string.IsNullOrWhiteSpace(search))
    {
        var term = search.Trim().ToLower();
        query = query.Where(x => x.Name.ToLower().Contains(term) || x.Sku.ToLower().Contains(term) || (x.Barcode != null && x.Barcode.Contains(term)));
    }

    var list = await query.OrderBy(x => x.Name).Select(x => new
    {
        x.Id, x.Name, x.Description, x.Sku, x.Barcode, x.CategoryId, x.BrandId,
        category = x.Category!.Name,
        brand = x.Brand != null ? x.Brand.Name : null,
        x.CostPrice, x.SalePrice, x.MinimumStock, x.IsActive,
        stock = x.Stock != null ? x.Stock.Quantity : 0
    }).ToListAsync();

    return Results.Ok(list);
});
products.MapGet("/{id:guid}", async (Guid id, MercadoDbContext db) =>
{
    var product = await db.Products.Include(x => x.Category).Include(x => x.Brand).Include(x => x.Stock)
        .FirstOrDefaultAsync(x => x.Id == id);
    return product is null ? Results.NotFound() : Results.Ok(product);
});
products.MapPost("", async (ProductRequest request, MercadoDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Trim().Length < 3)
        return Results.BadRequest("O nome do produto deve ter pelo menos 3 caracteres.");
    if (string.IsNullOrWhiteSpace(request.Sku)) return Results.BadRequest("SKU é obrigatório.");
    if (request.CostPrice < 0 || request.SalePrice < 0) return Results.BadRequest("Os preços não podem ser negativos.");
    if (request.MinimumStock < 0 || request.InitialStock < 0) return Results.BadRequest("O estoque não pode ser negativo.");
    if (!await db.Categories.AnyAsync(x => x.Id == request.CategoryId)) return Results.BadRequest("Categoria inválida.");
    if (request.BrandId.HasValue && !await db.Brands.AnyAsync(x => x.Id == request.BrandId.Value)) return Results.BadRequest("Marca inválida.");
    if (await db.Products.AnyAsync(x => x.Sku == request.Sku.Trim())) return Results.Conflict("Já existe um produto com este SKU.");
    if (!string.IsNullOrWhiteSpace(request.Barcode) && await db.Products.AnyAsync(x => x.Barcode == request.Barcode.Trim()))
        return Results.Conflict("Já existe um produto com este código de barras.");

    var product = new Product
    {
        Name = request.Name.Trim(),
        Description = request.Description?.Trim(),
        Sku = request.Sku.Trim(),
        Barcode = string.IsNullOrWhiteSpace(request.Barcode) ? null : request.Barcode.Trim(),
        CategoryId = request.CategoryId,
        BrandId = request.BrandId,
        CostPrice = request.CostPrice,
        SalePrice = request.SalePrice,
        MinimumStock = request.MinimumStock
    };
    db.Products.Add(product);
    db.StockItems.Add(new StockItem { ProductId = product.Id, Quantity = request.InitialStock });
    await db.SaveChangesAsync();
    return Results.Created($"/api/products/{product.Id}", product);
});
products.MapPut("/{id:guid}", async (Guid id, ProductRequest request, MercadoDbContext db) =>
{
    var product = await db.Products.Include(x => x.Stock).FirstOrDefaultAsync(x => x.Id == id);
    if (product is null) return Results.NotFound();
    if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Trim().Length < 3) return Results.BadRequest("Nome inválido.");
    if (request.CostPrice < 0 || request.SalePrice < 0 || request.MinimumStock < 0) return Results.BadRequest("Valores inválidos.");
    if (await db.Products.AnyAsync(x => x.Id != id && x.Sku == request.Sku.Trim())) return Results.Conflict("SKU já utilizado.");

    product.Name = request.Name.Trim();
    product.Description = request.Description?.Trim();
    product.Sku = request.Sku.Trim();
    product.Barcode = string.IsNullOrWhiteSpace(request.Barcode) ? null : request.Barcode.Trim();
    product.CategoryId = request.CategoryId;
    product.BrandId = request.BrandId;
    product.CostPrice = request.CostPrice;
    product.SalePrice = request.SalePrice;
    product.MinimumStock = request.MinimumStock;
    product.UpdatedAt = DateTime.UtcNow;
    await db.SaveChangesAsync();
    return Results.Ok(product);
});
products.MapPatch("/{id:guid}/status", async (Guid id, MercadoDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null) return Results.NotFound();
    product.IsActive = !product.IsActive;
    product.UpdatedAt = DateTime.UtcNow;
    await db.SaveChangesAsync();
    return Results.Ok(product);
});

// Inventory
api.MapGet("/inventory", async (MercadoDbContext db) =>
{
    var items = await db.StockItems.Include(x => x.Product).ThenInclude(x => x!.Category)
        .OrderBy(x => x.Product!.Name)
        .Select(x => new
        {
            x.ProductId,
            product = x.Product!.Name,
            sku = x.Product.Sku,
            quantity = x.Quantity,
            minimumStock = x.Product.MinimumStock,
            status = x.Quantity <= x.Product.MinimumStock ? "Baixo" : "Normal"
        }).ToListAsync();
    return Results.Ok(items);
});
api.MapPost("/inventory/{productId:guid}/adjust", async (Guid productId, decimal quantity, MercadoDbContext db) =>
{
    var stock = await db.StockItems.FirstOrDefaultAsync(x => x.ProductId == productId);
    if (stock is null) return Results.NotFound();
    if (stock.Quantity + quantity < 0) return Results.BadRequest("A operação deixaria o estoque negativo.");
    stock.Quantity += quantity;
    stock.UpdatedAt = DateTime.UtcNow;
    await db.SaveChangesAsync();
    return Results.Ok(stock);
});

// Suppliers and customers
var suppliers = api.MapGroup("/suppliers");
suppliers.MapGet("", async (MercadoDbContext db) => Results.Ok(await db.Suppliers.OrderBy(x => x.Name).ToListAsync()));
suppliers.MapPost("", async (SupplierRequest request, MercadoDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(request.Name)) return Results.BadRequest("Nome é obrigatório.");
    var supplier = new Supplier { Name = request.Name.Trim(), Document = request.Document?.Trim(), Phone = request.Phone?.Trim() };
    db.Suppliers.Add(supplier); await db.SaveChangesAsync(); return Results.Created($"/api/suppliers/{supplier.Id}", supplier);
});

var customers = api.MapGroup("/customers");
customers.MapGet("", async (MercadoDbContext db) => Results.Ok(await db.Customers.OrderBy(x => x.Name).ToListAsync()));
customers.MapPost("", async (CustomerRequest request, MercadoDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(request.Name)) return Results.BadRequest("Nome é obrigatório.");
    var customer = new Customer { Name = request.Name.Trim(), Document = request.Document?.Trim(), Phone = request.Phone?.Trim() };
    db.Customers.Add(customer); await db.SaveChangesAsync(); return Results.Created($"/api/customers/{customer.Id}", customer);
});

// Purchases
api.MapGet("/purchases", async (MercadoDbContext db) =>
{
    var list = await db.Purchases.Include(x => x.Supplier).Include(x => x.Items).ThenInclude(x => x.Product)
        .OrderByDescending(x => x.CreatedAt).Take(50).ToListAsync();
    return Results.Ok(list);
});
api.MapPost("/purchases", async (PurchaseRequest request, MercadoDbContext db) =>
{
    if (request.Items.Count == 0) return Results.BadRequest("Informe ao menos um item.");
    if (!await db.Suppliers.AnyAsync(x => x.Id == request.SupplierId)) return Results.BadRequest("Fornecedor inválido.");

    await using var transaction = await db.Database.BeginTransactionAsync();
    var purchase = new Purchase { SupplierId = request.SupplierId };
    foreach (var item in request.Items)
    {
        if (item.Quantity <= 0 || item.UnitPrice < 0) return Results.BadRequest("Quantidade e custo inválidos.");
        var product = await db.Products.Include(x => x.Stock).FirstOrDefaultAsync(x => x.Id == item.ProductId);
        if (product is null) return Results.BadRequest("Produto inválido.");
        purchase.Items.Add(new PurchaseItem { ProductId = item.ProductId, Quantity = item.Quantity, UnitCost = item.UnitPrice });
        purchase.Total += item.Quantity * item.UnitPrice;
        product.CostPrice = item.UnitPrice;
        product.UpdatedAt = DateTime.UtcNow;
        product.Stock!.Quantity += item.Quantity;
        product.Stock.UpdatedAt = DateTime.UtcNow;
    }
    db.Purchases.Add(purchase);
    await db.SaveChangesAsync();
    await transaction.CommitAsync();
    return Results.Created($"/api/purchases/{purchase.Id}", purchase);
});

// Sales
api.MapGet("/sales", async (MercadoDbContext db) =>
{
    var list = await db.Sales.Include(x => x.Customer).Include(x => x.Items).ThenInclude(x => x.Product)
        .OrderByDescending(x => x.CreatedAt).Take(50).ToListAsync();
    return Results.Ok(list);
});
api.MapPost("/sales", async (SaleRequest request, MercadoDbContext db) =>
{
    if (request.Items.Count == 0) return Results.BadRequest("Informe ao menos um item.");
    if (request.CustomerId.HasValue && !await db.Customers.AnyAsync(x => x.Id == request.CustomerId.Value))
        return Results.BadRequest("Cliente inválido.");

    await using var transaction = await db.Database.BeginTransactionAsync();
    var sale = new Sale { CustomerId = request.CustomerId, PaymentMethod = string.IsNullOrWhiteSpace(request.PaymentMethod) ? "Dinheiro" : request.PaymentMethod };

    foreach (var item in request.Items)
    {
        if (item.Quantity <= 0 || item.UnitPrice < 0) return Results.BadRequest("Quantidade e preço inválidos.");
        var product = await db.Products.Include(x => x.Stock).FirstOrDefaultAsync(x => x.Id == item.ProductId);
        if (product is null) return Results.BadRequest("Produto inválido.");
        if (!product.IsActive) return Results.BadRequest($"{product.Name} está inativo.");
        if (product.Stock is null || product.Stock.Quantity < item.Quantity)
            return Results.BadRequest($"Estoque insuficiente para {product.Name}.");

        sale.Items.Add(new SaleItem { ProductId = item.ProductId, Quantity = item.Quantity, UnitPrice = item.UnitPrice });
        sale.Total += item.Quantity * item.UnitPrice;
        product.Stock.Quantity -= item.Quantity;
        product.Stock.UpdatedAt = DateTime.UtcNow;
    }

    db.Sales.Add(sale);
    await db.SaveChangesAsync();
    await transaction.CommitAsync();
    return Results.Created($"/api/sales/{sale.Id}", sale);
});

app.Run();
