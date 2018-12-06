FROM alpine

RUN apk add --update --no-cache \
	lighttpd \
  gettext \
  && rm -rf /var/cache/apk/*

COPY lighttpd.conf /etc/lighttpd/lighttpd.conf
COPY /dist /var/www/atanet

RUN envsubst < /var/www/atanet/assets/configs/Development.config.json
RUN envsubst < /var/www/atanet/assets/configs/.config.json

EXPOSE 80

ENTRYPOINT ["lighttpd", "-D", "-f", "/etc/lighttpd/lighttpd.conf"]
