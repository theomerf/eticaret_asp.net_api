# SDK image (build aþamasý)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Çözüm dosyasýný kopyala
COPY ../ETicaret.sln ./

# Projeleri kopyala ve restore et
COPY ../Entities/ ./Entities/
COPY ../Services/ ./Services/
COPY ../Repositories/ ./Repositories/
COPY ./ ./Eticaret/

WORKDIR /app/Eticaret
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Runtime image (çalýþma zamaný)
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/ETicaret/out ./
ENTRYPOINT ["dotnet", "ETicaret.dll"]
