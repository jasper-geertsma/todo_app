# ---- Build Angular ----
FROM node:22-alpine AS build
WORKDIR /app

# Beter voor caching
COPY package*.json ./
RUN npm ci

# Rest van de code
COPY . .

# Optioneel: disable analytics voor CI
ENV NG_CLI_ANALYTICS=false

# Prod build (Angular 17+: dist/<project>/browser)
RUN npm run build -- --configuration production

# ---- Serve met Nginx ----
FROM nginx:alpine
# Statische site naar webroot
COPY --from=build /app/dist/todo-frontend/browser/ /usr/share/nginx/html/

# Vervang default server config door onze reverse proxy + SPA fallback
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
