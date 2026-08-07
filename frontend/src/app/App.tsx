import { AppProviders } from "@/app/providers/AppProviders";
import { AppRouter } from "@/app/routes/AppRouter";

// Composition root do frontend: encapsula os providers globais (React
// Query, futuramente autenticação/tema) em volta do roteador. Nenhuma
// tela/página real foi criada ainda — o roteador está preparado, mas
// vazio, aguardando os módulos de negócio.
export function App() {
  return (
    <AppProviders>
      <AppRouter />
    </AppProviders>
  );
}
