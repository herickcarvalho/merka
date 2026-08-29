Merka ERP

Sistema de gestão empresarial (ERP) desenvolvido para pequenas mercearias e mercados, reunindo em uma única aplicação o controle de produtos, estoque, compras, vendas, fornecedores e clientes.

O projeto foi desenvolvido como uma aplicação full stack e evoluiu de uma estrutura inicial focada em arquitetura para uma primeira versão funcional completa, com frontend, backend, banco de dados e deploy em nuvem.

Sobre o projeto

O Merka ERP nasceu como um projeto de portfólio com o objetivo de construir um sistema próximo de uma aplicação real, aplicando conceitos de arquitetura de software e regras de negócio.

O sistema foi pensado para centralizar as principais operações de uma pequena mercearia, evitando controles manuais e reduzindo problemas relacionados a estoque e vendas.

Funcionalidades implementadas

Dashboard com vendas do dia, produtos ativos e alertas de estoque baixo

Categorias e marcas

Cadastro e consulta de produtos

SKU e código de barras únicos

Produto ativo/inativo

Estoque inicial, estoque mínimo e ajuste manual

Fornecedores e clientes

Compras com entrada automática no estoque e atualização do custo

Vendas com baixa automática do estoque

Bloqueio de venda de produto inativo

Bloqueio de venda sem estoque suficiente

API REST documentada com Swagger

Frontend React responsivo

Health check da API

Banco de dados PostgreSQL

Tecnologias utilizadas

Backend

C#

.NET 8

ASP.NET Core

Entity Framework Core

Npgsql

PostgreSQL

Swagger / OpenAPI

Frontend

React

TypeScript

Vite

HTML5

CSS3

JavaScript

Banco de dados e infraestrutura

PostgreSQL

Neon

Docker

Docker Compose

Render

Git

GitHub

Arquitetura

O projeto preserva uma estrutura inspirada em Clean Architecture e organização modular.

A solução contém módulos separados para:

Products

Inventory

Sales

Purchasing

Identity

Também existem projetos compartilhados para responsabilidades transversais:

BuildingBlocks.Domain

BuildingBlocks.Application

BuildingBlocks.Infrastructure

A primeira versão funcional concentra parte da implementação no host da API para acelerar a entrega do MVP, enquanto a estrutura modular permanece preparada para evolução e expansão das funcionalidades.

Estrutura do projeto

merka-erp/
├── backend/
│   ├── src/
│   │   ├── Api/
│   │   │   └── Mercado.Api
│   │   ├── BuildingBlocks/
│   │   └── Modules/
│   │       ├── Identity/
│   │       ├── Inventory/
│   │       ├── Products/
│   │       ├── Purchasing/
│   │       └── Sales/
│   └── tests/
│
├── frontend/
├── docs/
└── docker-compose.yml

Regras de negócio implementadas

Produto não pode ser vendido estando inativo

Venda não pode ser realizada sem estoque suficiente

Compras atualizam automaticamente o estoque

Compras atualizam o custo do produto

Vendas realizam automaticamente a baixa do estoque

Produtos possuem identificação por SKU

Produtos podem possuir código de barras único

O sistema controla estoque mínimo e alertas relacionados

Executando com Docker

Na raiz do projeto:

cp .env.example .env
docker compose up --build

Após iniciar os serviços, acesse:

Frontend: http://localhost:5173

API/Swagger: http://localhost:5000/swagger

Health check: http://localhost:5000/health

Executando localmente

Banco de dados

É necessário um banco PostgreSQL 16 ou compatível.

Configure a connection string de acordo com o seu ambiente.

Exemplo:

Host=localhost;Port=5432;Database=mercado_erp;Username=mercado_user;Password=sua_senha

Backend

cd backend
dotnet restore
dotnet run --project src/Api/Mercado.Api

Frontend

Em outro terminal:

cd frontend
npm install
npm run dev

Deploy

O projeto foi preparado para utilização em ambiente de nuvem:

Banco de dados: Neon

Backend: Render

Controle de versão: GitHub

As configurações sensíveis, como a string de conexão do banco, devem ser utilizadas por meio de variáveis de ambiente.

ConnectionStrings__DefaultConnection
ASPNETCORE_ENVIRONMENT=Production

API

A documentação da API está disponível por meio do Swagger.

Localmente:

http://localhost:5000/swagger

Também existe um endpoint de health check:

http://localhost:5000/health

Próximos passos

Autenticação e autorização de usuários

Controle de permissões por perfil

Relatórios mais completos

Indicadores financeiros

Melhorias no dashboard

Auditoria de operações

Testes automatizados mais abrangentes

Migrations do Entity Framework Core

Melhorias na arquitetura modular

Evolução da experiência mobile

Objetivo do projeto

Mais do que criar um CRUD, o objetivo do Merka ERP foi desenvolver uma aplicação próxima de um cenário real, trabalhando com regras de negócio, arquitetura, persistência de dados, integração entre frontend e backend e publicação em ambiente de produção.

Autor

Herick Carvalho

GitHub: https://github.com/herickcarvalho

Projeto: https://github.com/herickcarvalho/merka
