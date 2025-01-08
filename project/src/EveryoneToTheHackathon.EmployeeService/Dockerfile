FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /project

COPY src/EveryoneToTheHackathon.Domain ./src/EveryoneToTheHackathon.Domain
COPY src/EveryoneToTheHackathon.Infrastructure ./src/EveryoneToTheHackathon.Infrastructure
RUN dotnet restore src/EveryoneToTheHackathon.Domain/EveryoneToTheHackathon.Domain.csproj
RUN dotnet restore src/EveryoneToTheHackathon.Infrastructure/EveryoneToTheHackathon.Infrastructure.csproj

COPY docs ./docs
COPY src/EveryoneToTheHackathon.EmployeeService ./src/EveryoneToTheHackathon.EmployeeService
RUN dotnet restore src/EveryoneToTheHackathon.EmployeeService/EveryoneToTheHackathon.EmployeeService.csproj

RUN dotnet publish ./src/EveryoneToTheHackathon.EmployeeService/EveryoneToTheHackathon.EmployeeService.csproj -c Release -o compiled

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /binaries

COPY --from=build /project/compiled .
COPY --from=build /project/docs ./docs
ENTRYPOINT ["dotnet", "EveryoneToTheHackathon.EmployeeService.dll"]