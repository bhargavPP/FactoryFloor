FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["services/identity-service/FactoryFloor.IdentityService/FactoryFloor.IdentityService.csproj","services/identity-service/FactoryFloor.IdentityService/"]

RUN dotnet restore "services/identity-service/FactoryFloor.IdentityService/FactoryFloor.IdentityService.csproj"

COPY . .

WORKDIR "/src/services/identity-service/FactoryFloor.IdentityService" 
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FactoryFloor.IdentityService.dll"]