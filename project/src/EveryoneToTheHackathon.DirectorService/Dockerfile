FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /project

COPY src ./src
COPY tests ./tests
COPY docs ./docs
COPY EveryoneToTheHackathon.sln . 
RUN dotnet restore

RUN dotnet publish -c Release -o compiled

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /binaries

COPY --from=build /project/compiled .
COPY --from=build /project/docs ./docs
ENTRYPOINT ["dotnet", "EveryoneToTheHackathon.Application.dll"]