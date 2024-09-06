FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HealthSystem.Identity/HealthSystem.Identity.csproj", "HealthSystem.Identity/"]
COPY ["HealthSystemCommon/HealthSystemCommon.csproj", "HealthSystemCommon/"]
RUN dotnet restore "./HealthSystem.Identity/HealthSystem.Identity.csproj"
COPY . .
WORKDIR "/src/HealthSystem.Identity"
RUN dotnet build "./HealthSystem.Identity.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HealthSystem.Identity.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthSystem.Identity.dll"]