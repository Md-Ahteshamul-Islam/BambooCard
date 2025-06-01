# Base build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and restore
COPY *.sln .
COPY src/ ./src/
COPY Plugins/ ./Plugins/

# Restore NuGet packages
WORKDIR /src/src/Presentation/Nop.Web
RUN dotnet restore

# Build and publish
RUN dotnet publish -c Release -o /app/publish

# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# Expose default port
EXPOSE 80

# Run nopCommerce
ENTRYPOINT ["dotnet", "Nop.Web.dll"]
