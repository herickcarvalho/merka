
# Business Rules Summary

- Products are never physically deleted.
- Products can be deactivated.
- Every product must belong to a category.
- Every product must belong to a brand.
- SKU must be unique.
- Barcode is optional.
- Sale price must be greater than zero.
- Cost price cannot be negative.
- Categories support hierarchical organization.
- The Product module is the foundation for Inventory, Sales, Purchases and Finance modules.


# Domain Model

## Objetivo

Este documento descreve as principais entidades de negócio do ERP para Mercearias.

O objetivo é documentar as regras do domínio antes da implementação, garantindo que o software seja construído sobre uma base consistente e de fácil manutenção.

---

# Módulo: Products

O módulo de Produtos é o núcleo do ERP.

Todos os demais módulos (Compras, Estoque, Vendas, Financeiro e Relatórios) dependem dele.

---

# Entidade: Product

Representa um produto comercializado pela empresa.

## Responsabilidades

- Identificar um produto.
- Armazenar informações comerciais.
- Controlar preço.
- Relacionar categoria.
- Relacionar marca.
- Servir de base para controle de estoque.

## Atributos

| Campo | Tipo | Obrigatório |
|--------|------|-------------|
| Id | Guid | Sim |
| Name | string | Sim |
| Description | string | Não |
| SKU | string | Sim |
| Barcode | string | Não |
| CategoryId | Guid | Sim |
| BrandId | Guid | Sim |
| UnitOfMeasure | Enum | Sim |
| CostPrice | decimal | Sim |
| SalePrice | decimal | Sim |
| MinimumStock | decimal | Sim |
| IsActive | bool | Sim |
| CreatedAt | DateTime | Sim |
| UpdatedAt | DateTime | Sim |

---

## Regras de Negócio

- Um produto nunca será removido fisicamente do banco de dados.
- Produtos podem ser desativados.
- SKU deve ser único.
- Código de barras pode ser vazio.
- O preço de venda deve ser maior que zero.
- O custo não pode ser negativo.
- Todo produto pertence a uma categoria.
- Todo produto pertence a uma marca.

---

# Entidade: Category

Representa a classificação de um produto.

## Atributos

- Id
- Name
- Description
- ParentCategoryId

## Regras

- Uma categoria pode possuir uma categoria pai.
- Uma categoria pode conter várias subcategorias.

---

# Entidade: Brand

Representa o fabricante do produto.

## Atributos

- Id
- Name

## Regras

- Uma marca pode possuir vários produtos.

---

# Enum: UnitOfMeasure

- UN
- KG
- G
- L
- ML
- CX
- FD
- PC

---

# Futuras Entidades

Ainda não serão implementadas.

- Inventory
- Supplier
- Purchase
- PurchaseItem
- Sale
- SaleItem
- Customer
- User
- CashRegister
- StockMovement
- PriceHistory

---

# Observações

Este documento será atualizado continuamente durante o desenvolvimento do ERP.

---

