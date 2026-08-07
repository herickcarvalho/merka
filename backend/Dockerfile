# syntax=docker/dockerfile:1

# ---------- Etapa 1: build ----------
# Usa a imagem SDK completa apenas para compilar; ela não vai para a
# imagem final, mantendo a imagem de runtime pequena e com menor
# superfície de ataque.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia primeiro apenas os arquivos de projeto/props para aproveitar o
# cache de camadas do Docker: "dotnet restore" só roda de novo se algum
# .csproj/.props mudar, não a cada alteração de código-fonte.
COPY Directory.Build.props Directory.Packages.props Mercado.sln ./
COPY src/BuildingBlocks/Mercado.BuildingBlocks.Domain/*.csproj src/BuildingBlocks/Mercado.BuildingBlocks.Domain/
COPY src/BuildingBlocks/Mercado.BuildingBlocks.Application/*.csproj src/BuildingBlocks/Mercado.BuildingBlocks.Application/
COPY src/BuildingBlocks/Mercado.BuildingBlocks.Infrastructure/*.csproj src/BuildingBlocks/Mercado.BuildingBlocks.Infrastructure/
COPY src/Modules/Products/Mercado.Modules.Products.Domain/*.csproj src/Modules/Products/Mercado.Modules.Products.Domain/
COPY src/Modules/Products/Mercado.Modules.Products.Application/*.csproj src/Modules/Products/Mercado.Modules.Products.Application/
COPY src/Modules/Products/Mercado.Modules.Products.Infrastructure/*.csproj src/Modules/Products/Mercado.Modules.Products.Infrastructure/
COPY src/Modules/Inventory/Mercado.Modules.Inventory.Domain/*.csproj src/Modules/Inventory/Mercado.Modules.Inventory.Domain/
COPY src/Modules/Inventory/Mercado.Modules.Inventory.Application/*.csproj src/Modules/Inventory/Mercado.Modules.Inventory.Application/
COPY src/Modules/Inventory/Mercado.Modules.Inventory.Infrastructure/*.csproj src/Modules/Inventory/Mercado.Modules.Inventory.Infrastructure/
COPY src/Modules/Sales/Mercado.Modules.Sales.Domain/*.csproj src/Modules/Sales/Mercado.Modules.Sales.Domain/
COPY src/Modules/Sales/Mercado.Modules.Sales.Application/*.csproj src/Modules/Sales/Mercado.Modules.Sales.Application/
COPY src/Modules/Sales/Mercado.Modules.Sales.Infrastructure/*.csproj src/Modules/Sales/Mercado.Modules.Sales.Infrastructure/
COPY src/Modules/Purchasing/Mercado.Modules.Purchasing.Domain/*.csproj src/Modules/Purchasing/Mercado.Modules.Purchasing.Domain/
COPY src/Modules/Purchasing/Mercado.Modules.Purchasing.Application/*.csproj src/Modules/Purchasing/Mercado.Modules.Purchasing.Application/
COPY src/Modules/Purchasing/Mercado.Modules.Purchasing.Infrastructure/*.csproj src/Modules/Purchasing/Mercado.Modules.Purchasing.Infrastructure/
COPY src/Modules/Identity/Mercado.Modules.Identity.Domain/*.csproj src/Modules/Identity/Mercado.Modules.Identity.Domain/
COPY src/Modules/Identity/Mercado.Modules.Identity.Application/*.csproj src/Modules/Identity/Mercado.Modules.Identity.Application/
COPY src/Modules/Identity/Mercado.Modules.Identity.Infrastructure/*.csproj src/Modules/Identity/Mercado.Modules.Identity.Infrastructure/
COPY src/Api/Mercado.Api/*.csproj src/Api/Mercado.Api/

RUN dotnet restore src/Api/Mercado.Api/Mercado.Api.csproj

# Agora copia o restante do código-fonte e publica
COPY src/ src/
RUN dotnet publish src/Api/Mercado.Api/Mercado.Api.csproj -c Release -o /app/publish --no-restore

# ---------- Etapa 2: runtime ----------
# Imagem final usa apenas o ASP.NET Runtime (bem menor que o SDK) e roda
# como usuário não-root, seguindo boas práticas de segurança em containers.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080

RUN useradd --uid 5678 --user-group --shell /bin/false appuser
USER appuser

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Mercado.Api.dll"]
