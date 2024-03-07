# Use the .NET 8 SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory in the container
WORKDIR /source
COPY . .

RUN dotnet restore "api.csproj" --disable-parallel
RUN dotnet publish "api.csproj" -c release -o /app --no-restore

# Build the runtime image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS runtime

# Set the working directory in the container
WORKDIR /app

# Copy the published output from the build environment
COPY --from=build /app ./

# Expose port 80 to the outside world
EXPOSE 5000

## Define the environment variables required for PostgreSQL connection
#ENV ConnectionStrings__DefaultConnection="Host=postgres;Port=5432;Database=MYN_Test_DB;Username=postgres;Password=start"
#
## Install PostgreSQL client
#RUN apt-get update && apt-get install -y postgresql-client

# Start the application
CMD ["dotnet", "Myn.GraphQL.Api.dll"]
