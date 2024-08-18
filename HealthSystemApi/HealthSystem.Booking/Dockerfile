FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HealthSystem.Booking/HealthSystem.Booking.csproj", "HealthSystem.Booking/"]
COPY ["HealthSystemCommon/HealthSystemCommon.csproj", "HealthSystemCommon/"]
RUN dotnet restore "./HealthSystem.Booking/HealthSystem.Booking.csproj"
COPY . .
WORKDIR "/src/HealthSystem.Booking"
RUN dotnet build "./HealthSystem.Booking.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HealthSystem.Booking.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthSystem.Booking.dll"]