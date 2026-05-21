FROM node:25-alpine3.22 AS base
WORKDIR /app
RUN npm install -g pnpm@10.30.3
COPY package.json pnpm-lock.yaml ./

FROM base AS development
RUN pnpm install
EXPOSE 5173
CMD ["pnpm", "run", "dev", "--", "--host"]

FROM base AS build
RUN pnpm install --frozen-lockfile
COPY . .
ARG VITE_API_URL
RUN pnpm run build

FROM nginx:alpine AS production
COPY nginx.conf /etc/nginx/conf.d/default.conf
RUN rm -rf /usr/share/nginx/html/*
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]