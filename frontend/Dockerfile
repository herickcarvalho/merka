# syntax=docker/dockerfile:1

# ---------- Etapa 1: build ----------
FROM node:22-alpine AS build
WORKDIR /app

# Copia apenas os manifestos primeiro para cache de camada do "npm ci"
COPY package.json package-lock.json* ./
RUN npm ci

COPY . .

# Permite injetar a URL da API em tempo de build (ver docker-compose.yml)
ARG VITE_API_BASE_URL=http://localhost:5000/api
ENV VITE_API_BASE_URL=$VITE_API_BASE_URL

RUN npm run build

# ---------- Etapa 2: runtime ----------
# Nginx serve apenas os arquivos estáticos gerados pelo Vite — a imagem
# final não contém Node.js, npm, nem código-fonte, apenas HTML/CSS/JS.
FROM nginx:1.27-alpine AS runtime
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
