# Stage 1 - build
FROM node:20 AS builder
RUN apt-get update && apt-get upgrade -y
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build

# Stage 2 - production nginx server
FROM nginx:alpine
COPY --from=builder /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
