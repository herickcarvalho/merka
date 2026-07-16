# Inventory.Infrastructure

Camada de infraestrutura do módulo **Inventory**.

- `Persistence/`   → `InventoryDbContext` (a ser criado), mapeamentos
  `IEntityTypeConfiguration<T>` e migrations do EF Core — isoladas por
  módulo (cada módulo tem seu próprio schema/DbContext, característica de
  monolito modular preparado para eventual extração em microsserviço).
- `Repositories/`  → Implementações concretas das interfaces definidas
  em `Inventory.Application/Abstractions` ou `Inventory.Domain/Repositories`.
- `DependencyInjection/` → Extension method `AddInventoryInfrastructure()`
  para registrar o DbContext e repositórios deste módulo.
