import axios from "axios";
import { env } from "@/core/config/env";

// Instância única do Axios usada por todos os módulos de negócio. Módulos
// não devem instanciar o próprio Axios — devem importar este cliente e
// criar, dentro de "modules/<modulo>/services/", funções específicas
// (ex.: getProducts(), createProduct()) que o utilizam.
export const httpClient = axios.create({
  baseURL: env.apiBaseUrl,
  timeout: 15000,
});

// Espaço reservado para interceptors futuros (ex.: anexar token de
// autenticação, tratar erro 401 globalmente) — nenhum implementado ainda.
