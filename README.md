# Mercado ERP

ERP modular para pequenas mercearias e mercados — projeto de portfólio
construído seguindo padrões de arquitetura usados em times sêniores.

> **Status:** apenas a base arquitetural (scaffolding) foi criada. Nenhuma
> regra de negócio, entidade, endpoint, tela ou autenticação foi
> implementada ainda — isso é intencional (ver `docs/ARCHITECTURE.md`).

## Stack

| Camada          | Tecnologia                                   |
|-----------------|-----------------------------------------------|
| Frontend        | React + TypeScript + Vite                     |
| Backend         | ASP.NET Core Web API (.NET 8) + EF Core       |
| Banco de dados  | PostgreSQL                                     |
| Infraestrutura  | Docker + Docker Compose                        |
| Qualidade       | ESLint, Prettier, EditorConfig, NetArchTest    |

## Estrutura do repositório

```
mercado-erp/
├── backend/     → API .NET em Clean Architecture + módulos de negócio
├── frontend/    → SPA React organizada por módulo de negócio
├── docs/        → Documentação de arquitetura
└── docker-compose.yml
```

Detalhes completos de cada camada e o porquê de cada decisão estão em
[`docs/ARCHITECTURE.md`](docs/ARCHITECTURE.md).

## Como rodar (ambiente de desenvolvimento)

1. Copie o arquivo de variáveis de ambiente:
   ```bash
   cp .env.example .env
   ```
2. Suba os três serviços (PostgreSQL, API, frontend):
   ```bash
   docker compose up --build
   ```
3. Acesse:
   - Frontend: http://localhost:5173
   - API (Swagger): http://localhost:5000/swagger
   - Health check da API: http://localhost:5000/health

### Rodando backend e frontend localmente (fora do Docker)

**Backend** (requer .NET 8 SDK):
```bash
cd backend
dotnet restore
dotnet run --project src/Api/Mercado.Api
```

**Frontend** (requer Node 22+):
```bash
cd frontend
npm install
npm run dev
```

## Qualidade de código

```bash
# Frontend
npm run lint        # ESLint
npm run format       # Prettier

# Backend
dotnet test backend/tests/Mercado.ArchitectureTests   # valida as regras de camadas
```

## Próximos passos

Com a base pronta, o desenvolvimento segue módulo a módulo (ex.: começar
por `Products`): modelar entidades no `Domain`, criar casos de uso no
`Application`, implementar persistência na `Infrastructure` e, por fim,
expor endpoints no host `Mercado.Api` — replicando exatamente o mesmo
padrão para `Inventory`, `Sales`, `Purchasing` e `Identity`.
