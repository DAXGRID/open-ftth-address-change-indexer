ARG PROJECT_NAME=OpenFTTH.AddressChangeIndexer
ARG DOTNET_VERSION=10.0

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION}-alpine AS build-env

# Renew the ARG argument for it to be available in this build context.
ARG PROJECT_NAME

WORKDIR /app

COPY ./*sln ./

COPY ./src/${PROJECT_NAME}/*.csproj ./src/${PROJECT_NAME}/
COPY ./test/${PROJECT_NAME}.Tests/*.csproj ./test/${PROJECT_NAME}.Tests/

RUN dotnet restore --packages ./packages

COPY . ./
WORKDIR /app/src/${PROJECT_NAME}
RUN dotnet publish -c Release -o out --packages ./packages

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:${DOTNET_VERSION}-alpine

# Renew the ARG argument for it to be available in this build context.
ARG PROJECT_NAME

WORKDIR /app

RUN apk add --no-cache icu-libs krb5-libs

COPY --from=build-env --chown=app:app /app/src/${PROJECT_NAME}/out .

USER app

ENTRYPOINT ["dotnet", "OpenFTTH.AddressChangeIndexer.dll"]
