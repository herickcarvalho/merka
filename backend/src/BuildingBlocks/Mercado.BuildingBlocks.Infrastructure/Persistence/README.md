# Persistence (BuildingBlocks.Infrastructure)

Esta pasta guarda convenções de persistência REUTILIZÁVEIS entre módulos
(ex.: uma classe base de configuração de auditoria, conversores de tipos
comuns do EF Core). Não contém nenhum DbContext — cada módulo de negócio
possui o seu próprio DbContext dentro de
`Modules/<Modulo>/Mercado.Modules.<Modulo>.Infrastructure/Persistence`.
