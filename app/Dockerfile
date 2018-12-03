FROM alpine

RUN apk add --update --no-cache \
	lighttpd \
  && rm -rf /var/cache/apk/*

COPY lighttpd.conf /etc/lighttpd/lighttpd.conf
COPY /dist/nc-license /var/www/license

EXPOSE 80

ENTRYPOINT ["lighttpd", "-D", "-f", "/etc/lighttpd/lighttpd.conf"]
