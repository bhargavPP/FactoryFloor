FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["services/notification-service/FactoryFloor.NotificationService/FactoryFloor.NotificationService.csproj","services/notification-service/FactoryFloor.NotificationService/"]
COPY ["shared/contracts/FactoryFloor.Contracts/FactoryFloor.Contracts.csproj","shared/contracts/FactoryFloor.Contracts/"]

RUN dotnet restore "services/notification-service/FactoryFloor.NotificationService/FactoryFloor.NotificationService.csproj"

COPY . .

WORKDIR "/src/services/notification-service/FactoryFloor.NotificationService"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FactoryFloor.NotificationService.dll"]