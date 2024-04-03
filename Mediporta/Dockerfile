
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS app-build
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet build -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS app-runtime
WORKDIR /app
COPY --from=app-build /app/out .
ENV TestDatabaseConnectionString="Server=database;Database=MediportaTaskDb;Trusted_Connection=True;Encrypt=False;"
ENTRYPOINT ["dotnet", "Mediporta.dll"]

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS database-build
WORKDIR /database
COPY Mediporta.Database/. .
RUN dotnet restore
RUN dotnet build -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS database-runtime
WORKDIR /database
COPY --from=database-build /database/out .
ENTRYPOINT ["dotnet", "Mediporta.Database.dll"]


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS tests-build
WORKDIR /tests
COPY Mediporta.Tests/. .
RUN dotnet restore
RUN dotnet build -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS tests-runtime
WORKDIR /tests
COPY --from=tests-build /tests/out .
ENTRYPOINT ["dotnet", "Mediporta.Tests.dll"]