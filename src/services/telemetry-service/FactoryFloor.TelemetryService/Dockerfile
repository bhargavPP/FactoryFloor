FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["services/telemetry-service/FactoryFloor.TelemetryService/FactoryFloor.TelemetryService.csproj","services/telemetry-service/FactoryFloor.TelemetryService/"]
COPY ["shared/contracts/FactoryFloor.Contracts/FactoryFloor.Contracts.csproj","shared/contracts/FactoryFloor.Contracts/"]

RUN dotnet restore "services/telemetry-service/FactoryFloor.TelemetryService/FactoryFloor.TelemetryService.csproj"

COPY . .

WORKDIR "/src/services/telemetry-service/FactoryFloor.TelemetryService"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FactoryFloor.TelemetryService.dll"]