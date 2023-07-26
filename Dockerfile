#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src

RUN apt-get -y update && apt-get -y upgrade \
    zip

#
# copy csproj and restore as distinct layers
COPY . .
RUN pwd
RUN ls -l
RUN dotnet restore repair-service-api/repair-service-api.csproj

WORKDIR /src/repair-service-api
RUN dotnet build repair-service-api.csproj -c Release -o /app/build

### run unit tests, failing container build if tests fail
WORKDIR /src/repair-service-unit-tests

RUN dotnet test --verbosity=normal --results-directory /app/publish --logger "trx;LogFileName=coverage.xml" repair-service-unit-tests.csproj

WORKDIR /src/integration-service-kafka-test

RUN dotnet test --verbosity=normal --results-directory /app/publish --logger "trx;LogFileName=coverage1.xml" integration-service-kafka-test.csproj


WORKDIR /src/repair-service-api
###

FROM build AS publish
RUN dotnet publish repair-service-api.csproj -c Release -o /app/publish

WORKDIR /app/publish
RUN zip -r app.zip *

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /src/invokeServices.sh /usr/start/invokeServices.sh
COPY --from=publish /app/publish/app.zip /usr/app/app.zip

# Workaround to support connecting to MongoAtlas 4.0 version from ASP.NETCORE 5.0
# ASPNETCORE 5.0 uses a cipher logic which is not supported in Mongo Altas 4.0
# Once the MongoAtlas Server is upgraded to 5.0, this step should be removed.
COPY openssl.cnf /etc/ssl/openssl.cnf


# Enable Datadog automatic instrumentation
# App is being copied to /app, so Datadog assets are at /app/datadog
ENV CORECLR_ENABLE_PROFILING=1
ENV CORECLR_PROFILER={846F5F1C-F9AE-4B07-969E-05C26BC060D8}
ENV CORECLR_PROFILER_PATH=/opt/datadog/Datadog.Trace.ClrProfiler.Native.so
ENV DD_INTEGRATIONS=/opt/datadog/integrations.json
ENV DD_DOTNET_TRACER_HOME=/opt/datadog
ENV TRACER_VERSION=2.4.1
ENV DD_LOGS_INJECTION=true

ADD https://github.com/DataDog/dd-trace-dotnet/releases/download/v${TRACER_VERSION}/datadog-dotnet-apm_${TRACER_VERSION}_amd64.deb ./
RUN dpkg -i ./datadog-dotnet-apm_${TRACER_VERSION}_amd64.deb && /opt/datadog/createLogPath.sh
RUN rm ./datadog-dotnet-apm_${TRACER_VERSION}_amd64.deb

RUN apt-get update -y && apt-get install -y \
    vim \
    dos2unix && apt-get clean

RUN dos2unix /usr/start/invokeServices.sh
RUN chmod +x /usr/start/invokeServices.sh

EXPOSE 80
ENTRYPOINT ["/usr/start/invokeServices.sh"]


