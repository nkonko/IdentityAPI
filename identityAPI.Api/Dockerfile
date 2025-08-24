# Etapa base: runtime de .NET
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true \
    DOTNET_CLI_TELEMETRY_OPTOUT=true

# Etapa build: SDK para compilar y ejecutar EF Core
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiamos solo los .csproj primero para aprovechar cache en restore
COPY ["identityAPI.Api/identityAPI.Api.csproj", "identityAPI.Api/"]
COPY ["identityAPI.Core/identityAPI.Core.csproj", "identityAPI.Core/"]
COPY ["identityAPI.Infrastructure/identityAPI.Infrastructure.csproj", "identityAPI.Infrastructure/"]

RUN dotnet restore "identityAPI.Api/identityAPI.Api.csproj"

# Copiamos todo el código y compilamos
COPY . .
WORKDIR "/src/identityAPI.Api"
RUN dotnet build "identityAPI.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa publish: genera artefactos optimizados
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "identityAPI.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final para desarrollo (SDK incluido para EF Core)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS final
WORKDIR /app

# Copiamos la salida del publish
COPY --from=publish /app/publish .

# Crear usuario no root
RUN adduser --disabled-password --home /app appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "identityAPI.Api.dll"]
