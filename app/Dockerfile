FROM alpine

RUN apk add --update --no-cache \
	lighttpd \
  gettext \
  && rm -rf /var/cache/apk/*

COPY lighttpd.conf /etc/lighttpd/lighttpd.conf
COPY /dist /var/www/atanet

EXPOSE 80

ENTRYPOINT ["lighttpd", "-D", "-f", "/etc/lighttpd/lighttpd.conf"]
