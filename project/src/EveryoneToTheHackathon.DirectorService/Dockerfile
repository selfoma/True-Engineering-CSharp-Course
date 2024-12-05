FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /project

COPY src/EveryoneToTheHackathon.Domain ./src/EveryoneToTheHackathon.Domain
COPY src/EveryoneToTheHackathon.Infrastructure ./src/EveryoneToTheHackathon.Infrastructure
RUN dotnet restore src/EveryoneToTheHackathon.Domain/EveryoneToTheHackathon.Domain.csproj
RUN dotnet restore src/EveryoneToTheHackathon.Infrastructure/EveryoneToTheHackathon.Infrastructure.csproj

COPY src/EveryoneToTheHackathon.DirectorService ./src/EveryoneToTheHackathon.DirectorService
RUN dotnet restore src/EveryoneToTheHackathon.DirectorService/EveryoneToTheHackathon.DirectorService.csproj

RUN dotnet publish ./src/EveryoneToTheHackathon.DirectorService/EveryoneToTheHackathon.DirectorService.csproj -c Release -o compiled

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /binaries

COPY --from=build /project/compiled .
ENTRYPOINT ["dotnet", "EveryoneToTheHackathon.DirectorService.dll"]