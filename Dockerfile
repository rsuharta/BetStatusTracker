# Build image
FROM microsoft/dotnet:2.1-sdk AS builder
ENV MONO_VERSION 5.4.1.6

RUN apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF

RUN echo "deb http://download.mono-project.com/repo/debian stretch/snapshots/$MONO_VERSION main" > /etc/apt/sources.list.d/mono-official.list \  
  && apt-get update \
  && apt-get install -y mono-runtime \
  && rm -rf /var/lib/apt/lists/* /tmp/*

RUN apt-get update \  
  && apt-get install -y binutils curl mono-devel ca-certificates-mono fsharp mono-vbnc nuget referenceassemblies-pcl \
  && rm -rf /var/lib/apt/lists/* /tmp/*

WORKDIR /sln

COPY ./build/build.sh ./build/build.cake ./

# Install Cake, and compile the Cake build script
RUN ./build.sh -t Clean

COPY ./BetStatusTracker.sln ./  
COPY ./src/BetStatusTracker/BetStatusTracker.csproj  ./src/BetStatusTracker/BetStatusTracker.csproj  
COPY ./src/BetStatusTracker.Repositories/BetStatusTracker.Repositories.csproj  ./src/BetStatusTracker.Repositories/BetStatusTracker.Repositories.csproj  
COPY ./src/BetStatusTracker.WebApi/BetStatusTracker.WebApi.csproj  ./src/BetStatusTracker.WebApi/BetStatusTracker.WebApi.csproj  
RUN ./build.sh -t Restore
RUN ./build.sh -t BuildWindowService

COPY ./src ./src

ARG PackageVersion
ENV PackageVersion=$PackageVersion

# Publish web app
RUN ./build.sh -t PackageWindowService

#App image
FROM microsoft/dotnet:2.1-runtime
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT Production
ENTRYPOINT ["dotnet", "BetStatusTracker.dll"]
COPY --from=builder ./sln/dist .