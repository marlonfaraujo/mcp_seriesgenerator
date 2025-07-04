# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/McpSeriesGenerator.App/McpSeriesGenerator.App.csproj", "src/McpSeriesGenerator.App/"]
RUN dotnet restore "./src/McpSeriesGenerator.App/McpSeriesGenerator.App.csproj"
COPY . .
WORKDIR "/src/src/McpSeriesGenerator.App"
RUN dotnet build "./McpSeriesGenerator.App.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./McpSeriesGenerator.App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
# write files in container with root by application
USER root 
WORKDIR /app
COPY --from=publish /app/publish .
RUN dotnet McpSeriesGenerator.App.dll &

ENV DOTNET_EnableDiagnostics=0
#ENTRYPOINT ["dotnet", "McpSeriesGenerator.App.dll"]

ENTRYPOINT ["tail", "-f", "/dev/null"]