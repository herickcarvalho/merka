using Mercado.Modules.Identity.Application.DependencyInjection;
using Mercado.Modules.Identity.Infrastructure.DependencyInjection;
using Mercado.Modules.Inventory.Application.DependencyInjection;
using Mercado.Modules.Inventory.Infrastructure.DependencyInjection;
using Mercado.Modules.Products.Application.DependencyInjection;
using Mercado.Modules.Products.Infrastructure.DependencyInjection;
using Mercado.Modules.Purchasing.Application.DependencyInjection;
using Mercado.Modules.Purchasing.Infrastructure.DependencyInjection;
using Mercado.Modules.Sales.Application.DependencyInjection;
using Mercado.Modules.Sales.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não configurada.");

// ---------------------------------------------------------------------
// Registro de cada módulo de negócio. O host não conhece detalhes
// internos de nenhum módulo, apenas chama os extension methods expostos
// por cada um (Application + Infrastructure). Isso é o que caracteriza
// um "monolito modular": módulos fisicamente separados em projetos, mas
// implantados juntos em um único processo — podendo ser extraídos para
// serviços independentes no futuro sem reescrever regra de negócio.
// ---------------------------------------------------------------------
builder.Services
    .AddProductsApplication().AddProductsInfrastructure(connectionString)
    .AddInventoryApplication().AddInventoryInfrastructure(connectionString)
    .AddSalesApplication().AddSalesInfrastructure(connectionString)
    .AddPurchasingApplication().AddPurchasingInfrastructure(connectionString)
    .AddIdentityApplication().AddIdentityInfrastructure(connectionString);

// ---------------------------------------------------------------------
// Infraestrutura transversal do host (Swagger, CORS, health checks).
// Nenhum endpoint de negócio é definido aqui — apenas os básicos de
// plataforma, prontos para os módulos mapearem seus próprios endpoints
// quando a implementação começar.
// ---------------------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString, name: "postgres");

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins(builder.Configuration["Cors:AllowedOrigin"] ?? "http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Frontend");

app.MapHealthChecks("/health");

// Os módulos mapearão seus próprios grupos de endpoints aqui quando
// implementados, ex.: app.MapProductsEndpoints();

app.Run();
