// Ponto único de leitura de variáveis de ambiente do Vite. Nenhum outro
// arquivo do projeto deve chamar "import.meta.env" diretamente — isso
// evita strings soltas espalhadas pelo código e centraliza validação.
export const env = {
  apiBaseUrl: import.meta.env.VITE_API_BASE_URL ?? "http://localhost:5000/api",
} as const;
