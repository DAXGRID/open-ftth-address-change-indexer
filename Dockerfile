ARG PROJECT_NAME=OpenFTTH.AddressChangeIndexer
ARG DOTNET_VERSION=8.0

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build-env

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
FROM mcr.microsoft.com/dotnet/runtime:${DOTNET_VERSION}

# Renew the ARG argument for it to be available in this build context.
ARG PROJECT_NAME

WORKDIR /app

COPY --from=build-env /app/src/${PROJECT_NAME}/out .

ENTRYPOINT ["dotnet", "OpenFTTH.AddressChangeIndexer.dll"]
