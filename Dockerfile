# --------------------------
# 1. Build aþamasý
# --------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Tüm proje dosyalarýný kopyala
COPY . .

# Restore ve publish
WORKDIR /src/Eticaret
RUN dotnet restore ../ETicaret.sln
RUN dotnet publish -c Release -o /app/out

# --------------------------
# 2. Runtime aþamasý
# --------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "Eticaret.dll"]
