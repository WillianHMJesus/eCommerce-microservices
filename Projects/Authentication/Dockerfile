FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV DOTNET_URLS=http://+:7234

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/EM.Authentication.API/EM.Authentication.API.csproj", "src/EM.Authentication.API/"]
COPY ["src/EM.Authentication.Application/EM.Authentication.Application.csproj", "src/EM.Authentication.Application/"]
COPY ["src/EM.Authentication.Domain/EM.Authentication.Domain.csproj", "src/EM.Authentication.Domain/"]
COPY ["src/EM.Authentication.Infraestructure/EM.Authentication.Infraestructure.csproj", "src/EM.Authentication.Infraestructure/"]
COPY . .
WORKDIR "/src/src/EM.Authentication.API"
RUN dotnet build "EM.Authentication.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EM.Authentication.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EM.Authentication.API.dll"]