FROM node:10.14.2 as builder

COPY . /tmp/atanet
WORKDIR /tmp/atanet
RUN npm i
RUN npm install -g @angular/cli@7.1.1
RUN ng build --prod --aot

FROM alpine:3.8

RUN apk add --update --no-cache \
	lighttpd \
  gettext \
  && rm -rf /var/cache/apk/*

RUN mkdir /var/www/atanet
COPY --from=builder /tmp/atanet/dist /var/www/atanet

COPY lighttpd.conf /etc/lighttpd/lighttpd.conf

EXPOSE 80

ENTRYPOINT ["lighttpd", "-D", "-f", "/etc/lighttpd/lighttpd.conf"]
