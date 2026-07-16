# Identity.Infrastructure

Camada de infraestrutura do módulo **Identity**.

- `Persistence/`   → `IdentityDbContext` (a ser criado), mapeamentos
  `IEntityTypeConfiguration<T>` e migrations do EF Core — isoladas por
  módulo (cada módulo tem seu próprio schema/DbContext, característica de
  monolito modular preparado para eventual extração em microsserviço).
- `Repositories/`  → Implementações concretas das interfaces definidas
  em `Identity.Application/Abstractions` ou `Identity.Domain/Repositories`.
- `DependencyInjection/` → Extension method `AddIdentityInfrastructure()`
  para registrar o DbContext e repositórios deste módulo.
