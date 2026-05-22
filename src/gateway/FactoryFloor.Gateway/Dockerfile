FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["gateway/FactoryFloor.Gateway/FactoryFloor.Gateway.csproj","gateway/FactoryFloor.Gateway/"]

RUN dotnet restore "gateway/FactoryFloor.Gateway/FactoryFloor.Gateway.csproj"

COPY . .

WORKDIR "/src/gateway/FactoryFloor.Gateway"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FactoryFloor.Gateway.dll"]