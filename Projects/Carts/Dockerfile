FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV DOTNET_URLS=http://+:7292

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Projects/Carts/src/EM.Carts.API/EM.Carts.API.csproj", "src/EM.Carts.API/"]
COPY ["Projects/Carts/src/EM.Carts.Application/EM.Carts.Application.csproj", "src/EM.Carts.Application/"]
COPY ["Projects/Carts/src/EM.Carts.Domain/EM.Carts.Domain.csproj", "src/EM.Carts.Domain/"]
COPY ["Projects/Carts/src/EM.Carts.Infraestructure/EM.Carts.Infraestructure.csproj", "src/EM.Carts.Infraestructure/"]
COPY ["Packages/.", "Packages"]
COPY ["Projects/Carts/nuget.config", "./"]
RUN dotnet restore "src/EM.Carts.API/EM.Carts.API.csproj" --configfile nuget.config --ignore-failed-sources
COPY ./Projects/Carts .
WORKDIR "/src/src/EM.Carts.API"
RUN dotnet build "EM.Carts.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EM.Carts.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EM.Carts.API.dll"]