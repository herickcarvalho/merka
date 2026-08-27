# core

Infraestrutura transversal do frontend — código técnico que não pertence
a nenhum módulo de negócio específico:

- `api/`     → Cliente HTTP compartilhado (instância única do Axios).
- `config/`  → Leitura centralizada de variáveis de ambiente.
- `errors/`  → Tipos de erro compartilhados entre módulos.

Regra: código em `core/` nunca deve importar de `modules/*` — a
dependência é sempre em uma via (modules → core).
