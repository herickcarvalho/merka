# Merka ERP

ERP MVP para pequenas mercearias e mercados.

## Funcionalidades implementadas

- Dashboard com vendas do dia, produtos ativos e alertas de estoque baixo
- Categorias e marcas
- Cadastro e consulta de produtos
- SKU e código de barras únicos
- Produto ativo/inativo
- Estoque inicial, estoque mínimo e ajuste manual
- Fornecedores e clientes
- Compras com entrada automática no estoque e atualização do custo
- Vendas com baixa automática do estoque
- Bloqueio de venda de produto inativo
- Bloqueio de venda sem estoque suficiente
- API REST documentada com Swagger
- Frontend React responsivo

## Rodar com Docker

Na raiz:

```bash
cp .env.example .env
docker compose up --build
```

Acesse:

- Frontend: http://localhost:5173
- API/Swagger: http://localhost:5000/swagger
- Health check: http://localhost:5000/health

## Rodar sem Docker

### Banco

É necessário PostgreSQL 16 ou compatível, com:

- Database: `mercado_erp`
- User: `mercado_user`
- Password: `change_me`

### Backend

```bash
cd backend
dotnet restore
dotnet run --project src/Api/Mercado.Api
```

### Frontend

```bash
cd frontend
npm install
npm run dev
```

## Observação sobre o banco

A API utiliza `EnsureCreated()` para criar automaticamente o schema na primeira execução. Para um projeto de produção, o próximo passo recomendado é substituir isso por migrations do EF Core.

## Arquitetura

O repositório preserva a estrutura inicial de módulos e Clean Architecture. O MVP funcional está concentrado no host da API para acelerar a entrega da primeira versão utilizável, enquanto a estrutura dos módulos permanece pronta para evolução.
