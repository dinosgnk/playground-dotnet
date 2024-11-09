FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /

# Copy .csproj and restore as distinct layers
COPY ./src/Services/Catalog/Catalog.API/Catalog.API.csproj ./Services/Catalog/Catalog.API/
RUN  dotnet restore ./Services/Catalog/Catalog.API/Catalog.API.csproj

# Copy everything else and build app
COPY ./src/Services/Catalog/Catalog.API/ ./Services/Catalog/Catalog.API/
WORKDIR /Services/Catalog/Catalog.API
RUN dotnet publish -c Release --no-restore -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0.10 AS final
WORKDIR /app
COPY --from=build /app  .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]
