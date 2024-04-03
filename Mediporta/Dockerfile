# APP
FROM mono:latest AS app-build
WORKDIR /app
COPY . .
RUN nuget restore && msbuild /p:Configuration=Release

FROM mono:latest AS app-runtime
WORKDIR /app
COPY --from=app-build /app/out .
ENTRYPOINT ["mono", "Mediporta/Mediporta.dll"]

# DATABASE
FROM mono:latest AS database-build
WORKDIR /app
COPY Mediporta.Database/. .
RUN nuget restore && msbuild /p:Configuration=Release

FROM mono:latest AS database-runtime
WORKDIR /app
COPY --from=database-build /app/out .
ENTRYPOINT ["mono", "Mediporta.Database.dll"]

# TESTS
FROM mono:latest AS tests-build
WORKDIR /app
COPY Mediporta.Tests/. .
RUN nuget restore && msbuild /p:Configuration=Release

FROM mono:latest AS tests-runtime
WORKDIR /app
COPY --from=tests-build /app .
ENTRYPOINT ["mono", "Mediporta.Tests.dll"]