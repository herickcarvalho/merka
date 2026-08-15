import { Routes, Route } from "react-router-dom";

// Roteador raiz da aplicação. Cada módulo de negócio (products, sales,
// inventory, purchasing) exporá seu próprio conjunto de rotas quando as
// telas forem implementadas (ex.: <Route path="/produtos/*"
// element={<ProductsRoutes />} />). Propositalmente vazio de páginas
// reais por enquanto.
export function AppRouter() {
  return (
    <Routes>
      {/* Exemplo futuro:
      <Route path="/produtos/*" element={<ProductsRoutes />} />
      <Route path="/vendas/*" element={<SalesRoutes />} />
      */}
    </Routes>
  );
}
