import type { PropsWithChildren } from "react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { BrowserRouter } from "react-router-dom";

// Todos os providers globais da aplicação ficam centralizados aqui, em
// um único lugar fácil de encontrar. À medida que o projeto crescer
// (ex.: contexto de autenticação, tema, i18n), novos providers entram
// nesta árvore sem precisar tocar em App.tsx.
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      refetchOnWindowFocus: false,
      retry: 1,
    },
  },
});

export function AppProviders({ children }: PropsWithChildren) {
  return (
    <QueryClientProvider client={queryClient}>
      <BrowserRouter>{children}</BrowserRouter>
    </QueryClientProvider>
  );
}
