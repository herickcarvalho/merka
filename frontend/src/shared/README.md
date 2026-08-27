# shared

Componentes, hooks e utilitários **genéricos e reutilizáveis** entre
múltiplos módulos de negócio (ex.: um componente `<Table>`, um hook
`useDebounce`, uma função `formatCurrency`). A regra que diferencia
`shared/` de `modules/<algo>/`: se o código menciona um conceito de
negócio específico do ERP (produto, venda, estoque), ele pertence ao
módulo; se é agnóstico de domínio, pertence aqui.

- `components/` → Componentes de UI puros (botões, inputs, modais).
- `hooks/`      → Hooks genéricos (ex.: useDebounce, useLocalStorage).
- `utils/`      → Funções utilitárias puras (formatação, datas).
- `types/`      → Tipos TypeScript genéricos compartilhados.
