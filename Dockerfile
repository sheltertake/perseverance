# --------------------------------------------------------------------------------------------------------------------------------------------
# 1 - stage build app
# --------------------------------------------------------------------------------------------------------------------------------------------
FROM node:14-buster-slim as build-app
WORKDIR /app

# install node modules
COPY ./frontend/package.json ./
COPY ./frontend/package-lock.json ./
RUN npm set progress=false && npm config set depth 0 && npm cache clean --force
## Storing node modules on a separate layer will prevent unnecessary npm installs at each build
RUN npm ci

# copy angular app
COPY ./frontend ./

## Build the angular app in production mode and store the artifacts in dist folder
## Demo app doesn't have lint
# RUN $(npm bin)/ng lint
RUN $(npm bin)/ng build --prod

# --------------------------------------------------------------------------------------------------------------------------------------------
# 2 - stage build api
# --------------------------------------------------------------------------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-api
WORKDIR /app

# copy backend
COPY ./backend ./

RUN dotnet restore 
RUN dotnet test -c Release
RUN dotnet publish -c Release -o /app/out


# --------------------------------------------------------------------------------------------------------------------------------------------
# 3 - stage runtime
# --------------------------------------------------------------------------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime

WORKDIR /app
COPY --from=build-api /app/out ./
# COPY --from=build-api /app/src/Perseverance.Proxy.Host/entrypoint.sh ./
COPY --from=build-app /app/dist/frontend ./wwwroot

# ENTRYPOINT ["/bin/bash", "/app/entrypoint.sh"]
ENTRYPOINT [ "dotnet", "Perseverance.Proxy.Host.dll" ]