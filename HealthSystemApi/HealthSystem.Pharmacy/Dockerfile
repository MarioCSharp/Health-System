FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HealthSystem.Pharmacy/HealthSystem.Pharmacy.csproj", "HealthSystem.Pharmacy/"]
COPY ["HealthSystemCommon/HealthSystemCommon.csproj", "HealthSystemCommon/"]
RUN dotnet restore "./HealthSystem.Pharmacy/HealthSystem.Pharmacy.csproj"
COPY . .
WORKDIR "/src/HealthSystem.Pharmacy"
RUN dotnet build "./HealthSystem.Pharmacy.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HealthSystem.Pharmacy.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthSystem.Pharmacy.dll"]