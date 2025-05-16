# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION={$BUILD_CONFIGURATION:-Release}
WORKDIR /src
COPY ["AgvAppMapStoreService/AgvAppMapStoreService/AgvAppMapStoreService.csproj", "AgvAppMapStoreService/"]
RUN dotnet restore "AgvAppMapStoreService/AgvAppMapStoreService.csproj"
COPY ./AgvAppMapStoreService .
WORKDIR "/src/AgvAppMapStoreService"
RUN mkdir -p /app/data && chown -R $USER:$USER /app/data
RUN dotnet build "AgvAppMapStoreService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION={$BUILD_CONFIGURATION:-Release}
RUN dotnet publish "AgvAppMapStoreService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Startup stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS startup
EXPOSE 8080
EXPOSE 8081
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "AgvAppMapStoreService.dll"]