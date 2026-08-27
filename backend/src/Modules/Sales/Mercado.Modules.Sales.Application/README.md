# Sales.Application

Camada de casos de uso do módulo **Sales**.

- `UseCases/`        → Um subdiretório por caso de uso (ex.:
  `CriarProduto/`, contendo Command, Handler e Validator juntos —
  organização "vertical slice", que facilita localizar tudo que envolve
  uma mesma operação de negócio).
- `Abstractions/`    → Interfaces que a Infrastructure deste módulo
  deve implementar (ex.: repositórios específicos do módulo).
- `DependencyInjection/` → Extension method `AddSalesApplication()`
  para registrar MediatR/Validators deste módulo no host da API.
