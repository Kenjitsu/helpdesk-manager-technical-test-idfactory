FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

EXPOSE 8080 

# BUILD 
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiamos solo los archivos .csproj primero para no descargar los paquetes NuGet cada vez que se cambia una línea de código.
COPY ["src/HelpDeskManager.API/HelpDeskManager.API.csproj", "src/HelpDeskManager.API/"]
COPY ["src/HelpDeskManager.BLL/HelpDeskManager.BLL.csproj", "src/HelpDeskManager.BLL/"]
COPY ["src/HelpDeskManager.Core/HelpDeskManager.Core.csproj", "src/HelpDeskManager.Core/"]
COPY ["src/HelpDeskManager.DAL/HelpDeskManager.DAL.csproj", "src/HelpDeskManager.DAL/"]

RUN dotnet restore "src/HelpDeskManager.API/HelpDeskManager.API.csproj"

COPY . .

WORKDIR "/src/src/HelpDeskManager.API"
RUN dotnet build "HelpDeskManager.API.csproj" -c Release -o /app/build

# PUBLISH
FROM build AS publish
RUN dotnet publish "HelpDeskManager.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# PRODUCCIÓN FINAL
FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "HelpDeskManager.API.dll"]