# Products.Infrastructure

Camada de infraestrutura do módulo **Products**.

- `Persistence/`   → `ProductsDbContext` (a ser criado), mapeamentos
  `IEntityTypeConfiguration<T>` e migrations do EF Core — isoladas por
  módulo (cada módulo tem seu próprio schema/DbContext, característica de
  monolito modular preparado para eventual extração em microsserviço).
- `Repositories/`  → Implementações concretas das interfaces definidas
  em `Products.Application/Abstractions` ou `Products.Domain/Repositories`.
- `DependencyInjection/` → Extension method `AddProductsInfrastructure()`
  para registrar o DbContext e repositórios deste módulo.
