# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copier solution et restaurer
COPY *.sln .
COPY src/DatingApp.Domain/DatingApp.Domain.csproj src/DatingApp.Domain/
COPY src/DatingApp.Application/DatingApp.Application.csproj src/DatingApp.Application/
COPY src/DatingApp.Infrastructure/DatingApp.Infrastructure.csproj src/DatingApp.Infrastructure/
COPY src/DatingApp.API/DatingApp.API.csproj src/DatingApp.API/

RUN dotnet restore

# Copier tout et build
COPY . .
RUN dotnet publish src/DatingApp.API/DatingApp.API.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 10000
ENV ASPNETCORE_URLS=http://+:10000

ENTRYPOINT ["dotnet", "DatingApp.API.dll"]