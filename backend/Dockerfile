FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and configuration files
COPY BookShare.slnx .
COPY Directory.Build.props .
COPY Directory.Packages.props .

# Copy project files
COPY BookShare.Core/*.csproj ./BookShare.Core/
COPY BookShare.Domain/*.csproj ./BookShare.Domain/
COPY BookShare.Infrastructure.Postgres/*.csproj ./BookShare.Infrastructure.Postgres/
COPY BookShare.Infrastructure.Storage/*.csproj ./BookShare.Infrastructure.Storage/

# Restore dependencies
RUN dotnet restore BookShare.slnx

# Copy full source code
COPY . .

# Build and publish
WORKDIR /src/BookShare.Core
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "BookShare.Core.dll"]
