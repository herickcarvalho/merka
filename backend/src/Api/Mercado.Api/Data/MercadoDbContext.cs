
using Mercado.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Mercado.Api.Data;

public class MercadoDbContext(DbContextOptions<MercadoDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<StockItem> StockItems => Set<StockItem>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Purchase> Purchases => Set<Purchase>();
    public DbSet<PurchaseItem> PurchaseItems => Set<PurchaseItem>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<SaleItem> SaleItems => Set<SaleItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<Brand>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<Product>().HasIndex(x => x.Sku).IsUnique();
        modelBuilder.Entity<Product>().HasIndex(x => x.Barcode).IsUnique().HasFilter("\"Barcode\" IS NOT NULL");
        modelBuilder.Entity<StockItem>().HasIndex(x => x.ProductId).IsUnique();

        modelBuilder.Entity<Product>().Property(x => x.CostPrice).HasPrecision(18, 2);
        modelBuilder.Entity<Product>().Property(x => x.SalePrice).HasPrecision(18, 2);
        modelBuilder.Entity<Purchase>().Property(x => x.Total).HasPrecision(18, 2);
        modelBuilder.Entity<PurchaseItem>().Property(x => x.Quantity).HasPrecision(18, 3);
        modelBuilder.Entity<PurchaseItem>().Property(x => x.UnitCost).HasPrecision(18, 2);
        modelBuilder.Entity<Sale>().Property(x => x.Total).HasPrecision(18, 2);
        modelBuilder.Entity<SaleItem>().Property(x => x.Quantity).HasPrecision(18, 3);
        modelBuilder.Entity<SaleItem>().Property(x => x.UnitPrice).HasPrecision(18, 2);

        modelBuilder.Entity<Product>()
            .HasOne(x => x.Stock)
            .WithOne(x => x.Product)
            .HasForeignKey<StockItem>(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PurchaseItem>().HasOne(x => x.Purchase).WithMany(x => x.Items).HasForeignKey(x => x.PurchaseId);
        modelBuilder.Entity<SaleItem>().HasOne(x => x.Sale).WithMany(x => x.Items).HasForeignKey(x => x.SaleId);
    }
}
