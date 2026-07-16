# Purchasing.Infrastructure

Camada de infraestrutura do módulo **Purchasing**.

- `Persistence/`   → `PurchasingDbContext` (a ser criado), mapeamentos
  `IEntityTypeConfiguration<T>` e migrations do EF Core — isoladas por
  módulo (cada módulo tem seu próprio schema/DbContext, característica de
  monolito modular preparado para eventual extração em microsserviço).
- `Repositories/`  → Implementações concretas das interfaces definidas
  em `Purchasing.Application/Abstractions` ou `Purchasing.Domain/Repositories`.
- `DependencyInjection/` → Extension method `AddPurchasingInfrastructure()`
  para registrar o DbContext e repositórios deste módulo.
