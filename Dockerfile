# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

# Copy solution and project files
COPY ["EngageGovContentMcp.sln", "./"]
COPY ["src/EngageGovContentMcp.Domain/EngageGovContentMcp.Domain.csproj", "src/EngageGovContentMcp.Domain/"]
COPY ["src/EngageGovContentMcp.Application/EngageGovContentMcp.Application.csproj", "src/EngageGovContentMcp.Application/"]
COPY ["src/EngageGovContentMcp.Infrastructure/EngageGovContentMcp.Infrastructure.csproj", "src/EngageGovContentMcp.Infrastructure/"]
COPY ["src/EngageGovContentMcp.Server/EngageGovContentMcp.Server.csproj", "src/EngageGovContentMcp.Server/"]

# Restore dependencies
RUN dotnet restore "EngageGovContentMcp.sln"

# Copy source code
COPY . .

# Build the application
WORKDIR "/src/src/EngageGovContentMcp.Server"
RUN dotnet build "EngageGovContentMcp.Server.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "EngageGovContentMcp.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
WORKDIR /app

# Create a non-root user for security
RUN addgroup -g 1000 appuser && \
    adduser -D -u 1000 -G appuser appuser && \
    chown -R appuser:appuser /app

# Install required runtime dependencies
RUN apk add --no-cache \
    icu-libs \
    icu-data-full

# Set environment variables for globalization
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8

# Copy published application
COPY --from=publish --chown=appuser:appuser /app/publish .

# Switch to non-root user
USER appuser

# Expose ports
EXPOSE 8080
EXPOSE 8081

# Configure ASP.NET Core to listen on all interfaces
ENV ASPNETCORE_URLS=http://+:8080

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD wget --no-verbose --tries=1 --spider http://localhost:8080/health || exit 1

# Entry point
ENTRYPOINT ["dotnet", "EngageGovContentMcp.Server.dll"]
