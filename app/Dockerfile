FROM node:8 as builder

COPY package.json package-lock.json ./

RUN npm set progress=false && npm config set depth 0 && npm cache clean --force

RUN npm i && mkdir /ng-app && cp -R ./node_modules ./ng-app

WORKDIR /ng-app

COPY . .

RUN apt-get update
RUN apt-get install -y gettext-base
ENV API_URL $API_URL
RUN envsubst < ./src/app/services/atanet-http.service.ts | tee ./src/app/services/atanet-http.service.ts

RUN $(npm bin)/ng build --prod

FROM nginx:1.13.3-alpine

COPY nginx/default.conf /etc/nginx/conf.d/

RUN rm -rf /usr/share/nginx/html/*

COPY --from=builder /ng-app/dist /usr/share/nginx/html
