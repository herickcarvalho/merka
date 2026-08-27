# modules

Cada subpasta representa um **módulo de negócio** do ERP, espelhando os
módulos do backend (`Products`, `Inventory`, `Sales`, `Purchasing`).
Organização "feature-first" (também chamada de vertical slice): em vez de
agrupar por TIPO de arquivo (todos os componentes juntos, todos os hooks
juntos), agrupa-se por FUNCIONALIDADE — o que facilita imensamente a
manutenção quando o projeto cresce para dezenas de telas.

Estrutura padrão dentro de cada módulo (ainda vazia, sem nenhuma tela ou
chamada de API implementada):

- `pages/`      → Componentes de página/rota do módulo.
- `components/` → Componentes de UI específicos deste módulo.
- `hooks/`      → Hooks específicos deste módulo (ex.: useProdutos).
- `services/`   → Chamadas HTTP do módulo, usando `core/api/httpClient`.
- `types/`      → Tipos TypeScript específicos do módulo (ex.: DTOs).

Regra de dependência: um módulo NUNCA deve importar diretamente de outro
módulo (`modules/sales` não importa de `modules/products`). Se dois
módulos precisam compartilhar algo, esse algo deve subir para `shared/`
ou `core/`.
