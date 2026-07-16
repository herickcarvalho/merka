# Sales.Infrastructure

Camada de infraestrutura do módulo **Sales**.

- `Persistence/`   → `SalesDbContext` (a ser criado), mapeamentos
  `IEntityTypeConfiguration<T>` e migrations do EF Core — isoladas por
  módulo (cada módulo tem seu próprio schema/DbContext, característica de
  monolito modular preparado para eventual extração em microsserviço).
- `Repositories/`  → Implementações concretas das interfaces definidas
  em `Sales.Application/Abstractions` ou `Sales.Domain/Repositories`.
- `DependencyInjection/` → Extension method `AddSalesInfrastructure()`
  para registrar o DbContext e repositórios deste módulo.
