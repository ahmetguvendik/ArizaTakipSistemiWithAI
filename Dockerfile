# /Presentation/WebApi/Dockerfile

# Stage 0: Base Image (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
# USER $APP_UID # Render veya base image tarafından tanımlanabilir, şimdilik yorum satırı yapıyorum.
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 1: Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Çözüm dosyasını kopyala
# Virgülleri kaldırdık ve yolu netleştirdik.
COPY "../../ArizaTakipSistemiWithAI.sln" "./"

# Proje dosyalarını kopyala (bağımlılık sırasına dikkat ederek)
# Virgülleri kaldırdık ve yolları netleştirdik.

# Presentation/WebApi/WebApi.csproj (kendi projemiz)
COPY "WebApi.csproj" "Presentation/WebApi/"

# Infrastructure/Persistance/Persistance.csproj
COPY "../../Infrastructure/Persistance/Persistance.csproj" "Infrastructure/Persistance/"

# Core/Domain/Domain.csproj
COPY "../../Core/Domain/Domain.csproj" "Core/Domain/"

# Core/Application/Application.csproj
COPY "../../Core/Application/Application.csproj" "Core/Application/"

# Frontend/DTO/DTO.csproj
COPY "../../Frontend/DTO/DTO.csproj" "Frontend/DTO/"

# NuGet paketlerini geri yükle
RUN dotnet restore "Presentation/WebApi/WebApi.csproj"

# Kalan tüm proje dosyalarını kopyala (kaynak kodlar)
# Dockerfile'ın bulunduğu dizindeki (Presentation/WebApi) tüm içeriği,
# /src/Presentation/WebApi dizinine kopyala.
COPY . "Presentation/WebApi/"

# Proje klasörüne geç
WORKDIR "/src/Presentation/WebApi"

# Uygulamayı derle
RUN dotnet build "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 2: Publish Stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 3: Final Stage (Runtime)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]