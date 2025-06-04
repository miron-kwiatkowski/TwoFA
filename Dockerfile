# Stage 1: Build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["TwoFA.csproj", "./"]
RUN dotnet restore "./TwoFA.csproj"

# Copy the rest of the code and publish
COPY . .
RUN dotnet publish "./TwoFA.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Create runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy published files from build stage
COPY --from=build /app/publish .

# Render provides the port in the PORT environment variable
ENV ASPNETCORE_URLS=http://*:${PORT}

# Expose port
EXPOSE 80

# Run the app
ENTRYPOINT ["dotnet", "TwoFA.dll"]
