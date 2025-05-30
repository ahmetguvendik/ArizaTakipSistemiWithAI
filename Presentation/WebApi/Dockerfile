# Stage 0: Base Image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ArizaTakipSistemiWithAI.sln ./

COPY Core/Domain/Domain.csproj Core/Domain/
COPY Core/Application/Application.csproj Core/Application/
COPY Infrastructure/Persistance/Persistance.csproj Infrastructure/Persistance/
COPY Frontend/DTO/DTO.csproj Frontend/DTO/
COPY Presentation/WebApi/WebApi.csproj Presentation/WebApi/

RUN dotnet restore Presentation/WebApi/WebApi.csproj

COPY . .

WORKDIR /src/Presentation/WebApi
RUN dotnet build WebApi.csproj -c $BUILD_CONFIGURATION -o /app/build

# Stage 2: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish WebApi.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 3: Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]
