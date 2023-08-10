# Use the base image for installing libpostal dependencies
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS libpostal_base
RUN apt-get update \
    && apt-get install -y curl autoconf automake libtool pkg-config git build-essential \
    && git clone https://github.com/openvenues/libpostal \
    && cd libpostal \
    && ./bootstrap.sh \
    && ./configure --datadir=/var/lib/ \
    && make -j4 \
    && make install \
    && ldconfig

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["LibPostalApi/LibPostalApi.csproj", "LibPostalApi/"]
COPY ["LibPostalNet/LibPostalNet.csproj", "LibPostalNet/"]
RUN dotnet restore "LibPostalApi/LibPostalApi.csproj"
COPY . .
WORKDIR "/src/LibPostalApi"
RUN dotnet build "LibPostalApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LibPostalApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use libpostal_base as the starting point for the final image
FROM libpostal_base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LibPostalApi.dll"]