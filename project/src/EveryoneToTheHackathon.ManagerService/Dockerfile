FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /project

COPY src/EveryoneToTheHackathon.Domain ./src/EveryoneToTheHackathon.Domain
COPY src/EveryoneToTheHackathon.Infrastructure ./src/EveryoneToTheHackathon.Infrastructure
RUN dotnet restore src/EveryoneToTheHackathon.Domain/EveryoneToTheHackathon.Domain.csproj
RUN dotnet restore src/EveryoneToTheHackathon.Infrastructure/EveryoneToTheHackathon.Infrastructure.csproj

COPY src/EveryoneToTheHackathon.ManagerService ./src/EveryoneToTheHackathon.ManagerService
RUN dotnet restore src/EveryoneToTheHackathon.ManagerService/EveryoneToTheHackathon.ManagerService.csproj

RUN dotnet publish ./src/EveryoneToTheHackathon.ManagerService/EveryoneToTheHackathon.ManagerService.csproj -c Release -o compiled

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /binaries

COPY --from=build /project/compiled .
ENTRYPOINT ["dotnet", "EveryoneToTheHackathon.ManagerService.dll"]