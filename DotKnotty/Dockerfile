# Use the official .NET SDK image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the files and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .

# Install SQLite
RUN apt-get update && apt-get install -y sqlite3 libsqlite3-dev

# Expose port 80 for the application
EXPOSE 80

# Set the entry point for the application
ENTRYPOINT ["sh", "-c", "dotnet DotKnotty.dll && dotnet ef database update"]