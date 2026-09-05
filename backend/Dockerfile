FROM mcr.microsoft.com/dotnet/sdk:10.0 AS restore

WORKDIR /src

COPY BookShare.slnx .
COPY Directory.Build.props .
COPY Directory.Packages.props .

COPY BookShare.Core/*.csproj BookShare.Core/
COPY BookShare.Domain/*.csproj BookShare.Domain/
COPY BookShare.Infrastructure.Postgres/*.csproj BookShare.Infrastructure.Postgres/
COPY BookShare.Infrastructure.Storage/*.csproj BookShare.Infrastructure.Storage/

RUN dotnet restore BookShare.slnx


FROM restore AS build

WORKDIR /src

COPY . .

RUN dotnet build BookShare.Core/BookShare.Core.csproj \
    -c Release \
    --no-restore


FROM build AS publish

RUN dotnet publish BookShare.Core/BookShare.Core.csproj \
    -c Release \
    -o /app/publish \
    --no-restore


FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "BookShare.Core.dll"]