FROM mcr.microsoft.com/mssql/server:2017-latest
ENV SA_PASSWORD=Passw0rd
ENV ACCEPT_EULA=Y

WORKDIR /usr/src/app

COPY EntryPoint.sh /usr/src/app
COPY SqlCmdStartup.sh /usr/src/app
COPY StartupScript.sql /usr/src/app

RUN chmod +x ./SqlCmdStartup.sh
CMD /bin/bash ./EntryPoint.sh