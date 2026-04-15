# STAGE 1: Build y Restore
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copiamos la solución y los proyectos manteniendo la estructura
COPY ["InventarioMicroservice.slnx", "./"]
COPY ["src/Inventario.API/Inventario.API.csproj", "src/Inventario.API/"]
COPY ["src/Inventario.Application/Inventario.Application.csproj", "src/Inventario.Application/"]
COPY ["src/Inventario.Domain/Inventario.Domain.csproj", "src/Inventario.Domain/"]
COPY ["src/Inventario.Infrastructure/Inventario.Infrastructure.csproj", "src/Inventario.Infrastructure/"]

# Restauramos las dependencias
RUN dotnet restore "src/Inventario.API/Inventario.API.csproj"

# Copiamos el resto del código
COPY . .

# Compilamos el proyecto de la API
WORKDIR "/app/src/Inventario.API"
RUN dotnet build "Inventario.API.csproj" -c Release -o /app/build

# STAGE 2: Publicación
FROM build AS publish
RUN dotnet publish "Inventario.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# STAGE 3: Runtime Final
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Exponemos el puerto por defecto de .NET
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Inventario.API.dll"]