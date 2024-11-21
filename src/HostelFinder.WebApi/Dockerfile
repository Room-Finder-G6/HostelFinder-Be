# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files to their correct locations
COPY ["src/HostelFinder.WebApi/HostelFinder.WebApi.csproj", "HostelFinder.WebApi/"]
COPY ["src/HostelFinder.Infrastructure/HostelFinder.Infrastructure.csproj", "HostelFinder.Infrastructure/"]
COPY ["src/HostelFinder.Application/HostelFinder.Application.csproj", "HostelFinder.Application/"]
COPY ["src/HostelFinder.Domain/HostelFinder.Domain.csproj", "HostelFinder.Domain/"]

# Restore dependencies
RUN dotnet restore "./HostelFinder.WebApi/HostelFinder.WebApi.csproj"

# Copy the remaining source code
COPY src/ .

# Build the project
WORKDIR "/src/HostelFinder.WebApi"
RUN dotnet build "./HostelFinder.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HostelFinder.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HostelFinder.WebApi.dll"]
