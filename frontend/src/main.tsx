import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { App } from "@/app/App";
import "@/styles/global.css";

// Ponto de entrada da SPA. Não contém nenhuma lógica de negócio — apenas
// monta a árvore React no elemento raiz do index.html.
createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <App />
  </StrictMode>,
);
