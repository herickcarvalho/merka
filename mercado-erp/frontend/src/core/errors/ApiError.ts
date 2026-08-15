// Tipo de erro padronizado para respostas de erro da API, usado pelos
// serviços de cada módulo para normalizar o formato de erro que a UI
// consome, independente de qual módulo do backend respondeu.
export class ApiError extends Error {
  constructor(
    message: string,
    public readonly statusCode?: number,
  ) {
    super(message);
    this.name = "ApiError";
  }
}
