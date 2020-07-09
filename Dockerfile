FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /src
COPY . .
RUN dotnet restore
WORKDIR /src/MediatonicPets
RUN dotnet build
WORKDIR /src
ENTRYPOINT ["tail", "-f", "/dev/null"]