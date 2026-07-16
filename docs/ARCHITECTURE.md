# Arquitetura — Mercado ERP

Este documento explica **o que** foi criado e, principalmente, **por quê**.
Nenhuma linha de regra de negócio existe neste momento — o objetivo desta
etapa é apenas a fundação: uma estrutura que suporte meses de evolução sem
precisar ser refeita.

---

## 1. Visão geral da decisão arquitetural

Foi adotado um **monolito modular em Clean Architecture**, e não uma API
"CRUD tradicional" (Controllers → Services → Models) nem microsserviços.
Motivos:

- **Microsserviços prematuros** são um dos erros mais comuns em projetos
  novos: trazem custo operacional (deploy, observabilidade, comunicação
  distribuída) sem necessidade real no estágio atual do produto.
- **Um monolito com Controllers/Services/Models genéricos** não escala
  organizacionalmente: à medida que o ERP ganha módulos (Produtos,
  Estoque, Vendas, Compras...), tudo se acumula nas mesmas pastas
  `Controllers/`, `Services/`, `Models/`, criando alto acoplamento entre
  áreas de negócio que não deveriam se conhecer.
- **Monolito modular** dá o melhor dos dois mundos: cada módulo de negócio
  é fisicamente isolado em seus próprios projetos (`.csproj`) com limites
  de dependência **verificados automaticamente em build** (via
  `NetArchTest`), mas tudo roda em um único processo/deploy — simples de
  operar agora, e com um caminho claro de extração para microsserviço no
  futuro, caso um módulo específico precise escalar independentemente.

---

## 2. Backend — Clean Architecture por módulo

### 2.1 As quatro camadas (por módulo)

Cada módulo de negócio (`Products`, `Inventory`, `Sales`, `Purchasing`,
`Identity`) é dividido em três projetos .NET, seguindo a regra de
dependência da Clean Architecture (as setas indicam "depende de"):

```
Domain  ←  Application  ←  Infrastructure
                              ↑
                        (Mercado.Api referencia
                         a Infrastructure de todos
                         os módulos e os conecta)
```

| Camada             | Responsabilidade                                              | Depende de              |
|--------------------|-----------------------------------------------------------------|--------------------------|
| **Domain**         | Entidades, Value Objects, eventos de domínio, regras de negócio puras | Nada (nem frameworks) |
| **Application**    | Casos de uso (Commands/Queries + Handlers), validação          | Domain                  |
| **Infrastructure** | EF Core, repositórios concretos, integrações externas          | Application              |
| **Api** (host)     | Program.cs, wiring de DI, Swagger, pipeline HTTP                | Infrastructure de todos os módulos |

A regra mais importante — e a que mais é violada em projetos que "crescem
de qualquer jeito" — é que **Domain nunca pode depender de nada**. Nem de
Entity Framework, nem de ASP.NET, nem de Application. Isso garante que a
regra de negócio pode ser testada em milissegundos (sem banco, sem HTTP) e
que trocar de EF Core para outro ORM no futuro não afeta uma linha de
regra de negócio.

Essa regra não fica apenas documentada — ela é **validada automaticamente**
pelo projeto `backend/tests/Mercado.ArchitectureTests`, que usa a
biblioteca NetArchTest para falhar o build de testes se, por exemplo,
alguém adicionar `using Microsoft.EntityFrameworkCore` dentro de um
projeto `*.Domain`. Isso substitui revisão manual de arquitetura em cada
Pull Request por uma verificação automática.

### 2.2 Por que "módulos" e não um único `Domain`/`Application`/`Infrastructure`?

Um ERP de mercearia tende a crescer para várias áreas de negócio bem
distintas: catálogo de produtos, controle de estoque, vendas/PDV, compras
com fornecedores, controle de acesso. Se todo o domínio ficasse em um
único projeto `Mercado.Domain`, o projeto se tornaria um "monólito dentro
do monólito" — qualquer desenvolvedor mexendo em Vendas teria toda a
complexidade de Estoque e Compras no mesmo namespace.

Separar por módulo (**vertical slice**, em vez de separar por tipo
técnico) significa que quem trabalha no módulo `Sales` não precisa
entender o código de `Purchasing` para fazer seu trabalho, e o link entre
módulos (quando necessário) é feito de forma explícita — tipicamente via
eventos de domínio — em vez de referências diretas de classe.

Módulos preparados nesta etapa (esqueleto apenas, sem entidades):

- **Products** — catálogo de produtos.
- **Inventory** — controle de estoque.
- **Sales** — vendas / ponto de venda.
- **Purchasing** — compras e fornecedores.
- **Identity** — reservado para autenticação/autorização futura (**nenhuma
  autenticação foi implementada**, conforme solicitado).

Novos módulos (ex.: `Financial`, `Reporting`) seguem exatamente o mesmo
padrão de 3 projetos + registro no host.

### 2.3 `BuildingBlocks` — o que é compartilhado entre módulos

Para não repetir código técnico em cada módulo (ex.: uma classe-base de
entidade), existe `BuildingBlocks/`, com a mesma divisão em três camadas,
mas contendo apenas **tipos-base genéricos, sem nenhuma regra de negócio**:

- `Mercado.BuildingBlocks.Domain` — `BaseEntity<TId>`, `AggregateRoot<TId>`,
  `ValueObject`, `IDomainEvent`, `DomainException`.
- `Mercado.BuildingBlocks.Application` — contratos de CQRS (`ICommand`,
  `IQuery`), `IUnitOfWork`, `IRepository<T>`, `Result`/`PagedResult`, e um
  `ValidationBehavior` (pipeline do MediatR que valida automaticamente
  todo Command/Query antes do Handler rodar).
- `Mercado.BuildingBlocks.Infrastructure` — convenções de persistência
  compartilhadas (ex.: interceptor de Domain Events).

Cada módulo referencia esses building blocks, mas os building blocks
**nunca** referenciam um módulo específico — o fluxo de dependência é
sempre módulo → building block, nunca o contrário.

### 2.4 CQRS leve com MediatR

A camada Application de cada módulo está preparada para o padrão CQRS
(Command Query Responsibility Segregation) usando MediatR — Commands para
escrita, Queries para leitura, cada um com seu Handler. Isso foi escolhido
em vez de "Services" genéricos porque:

- Cada caso de uso vira uma classe pequena e testável isoladamente (um
  Handler = uma responsabilidade), em vez de um `ProductService` gigante
  acumulando dezenas de métodos ao longo dos meses.
- Cross-cutting concerns (validação, logging, transação) entram como
  Pipeline Behaviors — um único `ValidationBehavior` já está pronto e vale
  para qualquer Command/Query de qualquer módulo, sem repetir código.

Nenhum Command/Query real foi criado ainda (isso seria implementar
funcionalidade), mas a pasta `UseCases/` de cada módulo já está pronta
para recebê-los, um subdiretório por caso de uso (ex.:
`UseCases/CriarProduto/CriarProdutoCommand.cs`,
`CriarProdutoHandler.cs`, `CriarProdutoValidator.cs` juntos).

### 2.5 Central Package Management

O arquivo `backend/Directory.Packages.props` centraliza a **versão** de
cada pacote NuGet usado pela solução; os `.csproj` individuais referenciam
pacotes sem especificar versão. Com ~20 projetos na solução, isso evita o
problema clássico de dois módulos usando versões diferentes do mesmo
pacote (ex.: EF Core 8.0.10 em um módulo e 8.0.7 em outro), algo que só
seria percebido em tempo de build/runtime sem essa prática.

### 2.6 Testes

Três projetos de teste, com responsabilidades diferentes:

- **`Mercado.ArchitectureTests`** — não testa regra de negócio, testa a
  própria arquitetura (ver seção 2.1).
- **`Mercado.UnitTests`** — testes unitários rápidos, sem I/O.
- **`Mercado.IntegrationTests`** — preparado para usar
  `Microsoft.AspNetCore.Mvc.Testing` + `Testcontainers.PostgreSql`, ou
  seja, sobe a API real contra um PostgreSQL descartável em container,
  garantindo testes de integração que não dependem de um banco
  compartilhado nem de mocks de banco de dados.

### 2.7 Dockerfile do backend

Build multi-stage: a etapa `build` usa a imagem `sdk` completa (necessária
só para compilar), e a imagem final usa apenas `aspnet` (runtime), rodando
como usuário não-root. Isso reduz o tamanho final da imagem e a superfície
de ataque — prática padrão em times que levam produção a sério.

---

## 3. Frontend — organização por módulo (feature-first)

### 3.1 Por que não "components/", "pages/", "hooks/" na raiz?

A estrutura tradicional de projetos iniciantes agrupa arquivos por
**tipo técnico**: uma pasta com todos os componentes, outra com todos os
hooks, outra com todas as páginas. Isso funciona bem em projetos pequenos,
mas se torna difícil de navegar assim que o número de telas cresce — para
entender tudo sobre "Vendas" seria preciso caçar arquivos espalhados em
4-5 pastas diferentes.

Este projeto usa organização **feature-first** (também chamada de
"vertical slice"): cada módulo de negócio (`products`, `inventory`,
`sales`, `purchasing`) tem sua própria pasta em `src/modules/`, contendo
`components/`, `hooks/`, `pages/`, `services/` e `types/` **daquele
módulo apenas**. Isso espelha exatamente a divisão em módulos do backend,
o que facilita a navegação mental entre as duas pontas do sistema.

### 3.2 As quatro áreas de `src/`

```
src/
├── app/       → composição raiz: App.tsx, providers globais, roteador
├── modules/   → um subdiretório por módulo de negócio (feature-first)
├── shared/    → componentes/hooks/utils REUTILIZÁVEIS entre módulos
└── core/      → infraestrutura técnica (cliente HTTP, config, erros)
```

Regra de dependência (análoga à do backend): `modules/*` pode importar de
`shared/` e `core/`, mas **nunca de outro módulo** (`modules/sales` não
importa de `modules/products`). `shared/` e `core/` nunca importam de
`modules/*`. Se dois módulos precisarem compartilhar algo, esse algo sobe
para `shared/`.

A diferença entre `shared/` e `core/`: `shared/` é sobre **UI e utilitários
reutilizáveis** (um componente de tabela, uma função de formatação de
moeda); `core/` é sobre **infraestrutura técnica** (o cliente HTTP único,
leitura de variáveis de ambiente, tipos de erro de API).

### 3.3 Decisões técnicas do frontend

- **Vite** como bundler/dev server — já solicitado, e é o padrão atual
  para SPAs React por causa da velocidade de HMR.
- **TypeScript em modo estrito** (`strict: true`, `noUnusedLocals`,
  `noUnusedParameters`) — pega erros em tempo de compilação em vez de
  runtime, essencial em um ERP onde erros de tipo em cálculos/valores
  monetários são caros.
- **Aliases de import (`@/modules/...`, `@/shared/...`)** configurados
  tanto no `tsconfig.app.json` quanto no `vite.config.ts` (precisam
  bater nos dois lugares) — evita imports frágeis como
  `../../../../shared/utils`.
- **TanStack React Query** já preparado no `AppProviders` como camada de
  cache/estado de servidor — decisão de infraestrutura (não é regra de
  negócio) que evita reinventar cache, loading e retry manualmente em
  cada módulo quando as chamadas de API começarem a ser implementadas.
- **Axios centralizado em `core/api/httpClient.ts`** — um único ponto para
  futuramente adicionar interceptors (ex.: anexar token de autenticação),
  em vez de cada módulo criar sua própria instância HTTP.
- **ESLint 9 (flat config) + Prettier**, com `eslint-config-prettier`
  desligando regras de estilo do ESLint que colidiriam com o Prettier —
  cada ferramenta cuida de uma responsabilidade (ESLint = qualidade/bugs
  potenciais, Prettier = formatação), sem sobreposição.
- **Dockerfile multi-stage**: build com Node (`npm run build`) e runtime
  final servido por Nginx puro — a imagem final não contém Node.js nem
  código-fonte, apenas os arquivos estáticos gerados.

---

## 4. Infraestrutura (Docker Compose)

`docker-compose.yml` na raiz orquestra três serviços para desenvolvimento
local: `postgres`, `api` e `frontend`, cada um com seu próprio Dockerfile.
O serviço `api` só sobe depois que o `postgres` reporta saudável
(`healthcheck` com `pg_isready`), evitando erros de conexão na
inicialização. Variáveis sensíveis (senha do banco, connection string)
vêm de um arquivo `.env` (nunca commitado — apenas `.env.example` serve
de referência).

Em produção, o mais comum seria implantar cada serviço separadamente (ex.:
banco gerenciado, API e frontend em serviços distintos), mas manter um
compose único aqui cobre o objetivo desta etapa: permitir rodar o projeto
completo localmente com um único comando.

---

## 5. Qualidade de código

- **EditorConfig** (`.editorconfig`) — garante indentação/charset/quebra de
  linha consistentes entre C# e TypeScript, independente da IDE de cada
  desenvolvedor.
- **ESLint + Prettier** (frontend) — já detalhado na seção 3.3.
- **Analisadores nativos do .NET** (`EnableNETAnalyzers`,
  `EnforceCodeStyleInBuild` em `Directory.Build.props`) — equivalente do
  ESLint para o lado do backend, ativado centralmente para toda a
  solução.
- **NetArchTest** — validação automática das regras de camada (seção
  2.1), o item mais importante para impedir que a arquitetura se degrade
  ao longo dos meses de desenvolvimento.

---

## 6. O que **não** foi feito (e por quê)

Por definição do escopo desta etapa, os seguintes itens foram
propositalmente deixados de fora, para serem implementados módulo a
módulo nas próximas etapas do projeto:

- Nenhuma entidade de domínio, Value Object ou regra de negócio real.
- Nenhum endpoint HTTP de negócio (apenas `/health` existe, como
  infraestrutura).
- Nenhuma tela ou componente de UI de negócio.
- Nenhuma autenticação/autorização (o módulo `Identity` existe apenas como
  esqueleto reservado).
- Nenhuma migration do EF Core (não há `DbContext` concreto ainda — cada
  módulo criará o seu quando suas entidades forem modeladas).

O objetivo desta etapa era exclusivamente entregar uma base sólida,
documentada e com padrões automaticamente verificáveis (build + testes de
arquitetura), pronta para receber meses de desenvolvimento de forma
organizada.
