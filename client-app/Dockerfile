FROM node:16 as build-stage
WORKDIR /app
COPY client-app/package*.json ./
RUN npm install
COPY client-app/ .
RUN npm run build

FROM nginx:stable-alpine as production-stage
COPY --from=build-stage /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]