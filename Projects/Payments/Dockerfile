FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# ENV DOTNET_URLS=http://+:7292

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Projects/Payments/src/EM.Payments.Worker/EM.Payments.Worker.csproj", "src/EM.Payments.Worker/"]
COPY ["Projects/Payments/src/EM.Payments.Application/EM.Payments.Application.csproj", "src/EM.Payments.Application/"]
COPY ["Projects/Payments/src/EM.Payments.Domain/EM.Payments.Domain.csproj", "src/EM.Payments.Domain/"]
COPY ["Projects/Payments/src/EM.Payments.Infraestructure/EM.Payments.Infraestructure.csproj", "src/EM.Payments.Infraestructure/"]
COPY ["Packages/.", "Packages"]
COPY ["Projects/Payments/nuget.config", "./"]
RUN dotnet restore "src/EM.Payments.Worker/EM.Payments.Worker.csproj" --configfile nuget.config --ignore-failed-sources
COPY ./Projects/Payments .
WORKDIR "/src/src/EM.Payments.Worker"
RUN dotnet build "EM.Payments.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EM.Payments.Worker.csproj" -c Release -o /app/publish
# RUN dotnet publish "EM.Payments.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
# COPY --from=publish /app/out .
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EM.Payments.Worker.dll"]