# Multi-stage build: the previous Dockerfile only copied a pre-published output.
# This one builds the server from source so `docker build` works standalone.
#
# Note: the bundled Angular admin UI (dangl-opencde-ui) is NOT built here, so the
# human-facing hand-shake pages (/opencde-select-documents, /opencde-upload-documents)
# will 404 -- this image is meant as an API test double for BIM-Guard's Documents API
# client, not a full interactive CDE. The API/controller routes are unaffected.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/server/Dangl.OpenCDE/Dangl.OpenCDE.csproj src/server/Dangl.OpenCDE/
COPY src/server/Dangl.OpenCDE.Core/Dangl.OpenCDE.Core.csproj src/server/Dangl.OpenCDE.Core/
COPY src/server/Dangl.OpenCDE.Data/Dangl.OpenCDE.Data.csproj src/server/Dangl.OpenCDE.Data/
COPY src/server/Dangl.OpenCDE.Shared/Dangl.OpenCDE.Shared.csproj src/server/Dangl.OpenCDE.Shared/
RUN dotnet restore src/server/Dangl.OpenCDE/Dangl.OpenCDE.csproj

COPY src/server/Dangl.OpenCDE/ src/server/Dangl.OpenCDE/
COPY src/server/Dangl.OpenCDE.Core/ src/server/Dangl.OpenCDE.Core/
COPY src/server/Dangl.OpenCDE.Data/ src/server/Dangl.OpenCDE.Data/
COPY src/server/Dangl.OpenCDE.Shared/ src/server/Dangl.OpenCDE.Shared/

RUN dotnet publish src/server/Dangl.OpenCDE/Dangl.OpenCDE.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app .
EXPOSE 80
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ENV ASPNETCORE_URLS=http://*:80
ENTRYPOINT ["dotnet", "Dangl.OpenCDE.dll"]
