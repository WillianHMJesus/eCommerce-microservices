FROM mcr.microsoft.com/mssql/server:2017-latest
ENV SA_PASSWORD=Passw0rd
ENV ACCEPT_EULA=Y

WORKDIR /usr/src/app

COPY . /usr/src/app

RUN chmod +x ./SqlCmdStartup.sh
CMD /bin/bash ./EntryPoint.sh