FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV DOTNET_URLS=http://+:7274

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Projects/Catalogs/src/EM.Catalog.API/EM.Catalog.API.csproj", "src/EM.Catalog.API/"]
COPY ["Projects/Catalogs/src/EM.Catalog.Application/EM.Catalog.Application.csproj", "src/EM.Catalog.Application/"]
COPY ["Projects/Catalogs/src/EM.Catalog.Domain/EM.Catalog.Domain.csproj", "src/EM.Catalog.Domain/"]
COPY ["Projects/Catalogs/src/EM.Catalog.Infraestructure/EM.Catalog.Infraestructure.csproj", "src/EM.Catalog.Infraestructure/"]
COPY ["Packages/.", "Packages"]
COPY ["Projects/Catalogs/nuget.config", "./"]
RUN dotnet restore "src/EM.Catalog.API/EM.Catalog.API.csproj" --configfile nuget.config --ignore-failed-sources
COPY ./Projects/Catalogs .
WORKDIR "/src/src/EM.Catalog.API"
RUN dotnet build "EM.Catalog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EM.Catalog.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EM.Catalog.API.dll"]