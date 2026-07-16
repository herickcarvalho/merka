import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import path from "node:path";

// Configuração do Vite: plugin do React (Fast Refresh) + os mesmos
// aliases de import definidos no tsconfig.app.json, para que
// "@/modules/..." funcione tanto para o compilador TS quanto para o
// bundler em tempo de execução.
export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      "@": path.resolve(__dirname, "./src"),
      "@/app": path.resolve(__dirname, "./src/app"),
      "@/modules": path.resolve(__dirname, "./src/modules"),
      "@/shared": path.resolve(__dirname, "./src/shared"),
      "@/core": path.resolve(__dirname, "./src/core"),
    },
  },
  server: {
    port: 5173,
    host: true,
  },
});
