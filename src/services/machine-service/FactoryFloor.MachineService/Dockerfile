FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["services/machine-service/FactoryFloor.MachineService/FactoryFloor.MachineService.csproj","services/machine-service/FactoryFloor.MachineService/"]
COPY ["shared/contracts/FactoryFloor.Contracts/FactoryFloor.Contracts.csproj","shared/contracts/FactoryFloor.Contracts/"]
RUN dotnet restore "services/machine-service/FactoryFloor.MachineService/FactoryFloor.MachineService.csproj"

COPY . .

WORKDIR "/src/services/machine-service/FactoryFloor.MachineService"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FactoryFloor.MachineService.dll"]