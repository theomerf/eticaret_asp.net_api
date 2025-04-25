# Build aþamasý
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Tüm proje dosyalarýný konteynýra kopyala
COPY . .

# Projeyi restore et
RUN dotnet restore ./ETicaret.csproj

# Yayýn (publish) iþlemi
RUN dotnet publish ./ETicaret.csproj -c Release -o /app/publish

# Runtime aþamasý
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Yayýnlanan dosyalarý kopyala
COPY --from=build /app/publish .

# Uygulamayý baþlat
ENTRYPOINT ["dotnet", "ETicaret.dll"]
