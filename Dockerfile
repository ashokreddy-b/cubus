# Use the official .NET Core SDK image as the build environment
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env

# Set the working directory inside the container
WORKDIR /app

# Copy the .csproj files and restore any dependencies (this step can be cached)
COPY LogFileAnalysis/*.csproj ./
RUN dotnet restore

# Copy the remaining application source code
COPY . ./

# Build the application
RUN dotnet publish -c Release -o out

# Use the official .NET Core runtime image for the final image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the published application from the build environment
COPY --from=build-env /app/out .

# Expose the port your application listens on
EXPOSE 80

# Define the entry point for your application
ENTRYPOINT ["dotnet", "LogFileAnalysis.dll"]
