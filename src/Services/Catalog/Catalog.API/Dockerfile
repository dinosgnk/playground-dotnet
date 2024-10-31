FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder

WORKDIR /

# # Setup working directory for the project
# COPY ./src/Services/Catalog/Catalog.API/Catalog.API.csproj ./Services/Catalog/Catalog.API/

# # Restore nuget packages
# RUN  dotnet restore ./Services/Catalog/Catalog.API/

# # Copy project files
# COPY ./src/Services/Catalog/Catalog.API/ ./Services/Catalog/Catalog.API/

# WORKDIR /Services/Catalog/Catalog.API

# # Publish project to output folder
# RUN dotnet publish -c Release --no-build -o publish

# FROM mcr.microsoft.com/dotnet/aspnet:8.0

# # Setup working directory for the project
# WORKDIR /app
# COPY --from=builder /Services/Catalog/Catalog.API/out  .

# ENTRYPOINT ["dotnet", "Catalog.API.dll"]
