FROM mcr.microsoft.com/mssql/server:2017-latest
ENV SA_PASSWORD=Passw0rd
ENV ACCEPT_EULA=Y

RUN mkdir -p /usr/config
WORKDIR /usr/config

COPY . /usr/config

RUN chmod +x /usr/config/entrypoint.sh
RUN chmod +x /usr/config/configure-db.sh

ENTRYPOINT ["./entrypoint.sh"]