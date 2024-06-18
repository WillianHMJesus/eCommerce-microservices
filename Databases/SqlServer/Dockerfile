FROM mcr.microsoft.com/mssql/server:2022-latest
ENV SA_PASSWORD=Passw0rd
ENV ACCEPT_EULA=Y

USER root

RUN mkdir -p /usr/config
RUN chown mssql /usr/config
WORKDIR /usr/config

COPY . /usr/config

RUN chmod +x /usr/config/entrypoint.sh
RUN chmod +x /usr/config/configure-db.sh

USER mssql

ENTRYPOINT ["./entrypoint.sh"]