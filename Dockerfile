# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar arquivos de projeto e restaurar dependências
COPY ["src/OrderSystem.Api/OrderSystem.Api.csproj", "src/OrderSystem.Api/"]
RUN dotnet restore "src/OrderSystem.Api/OrderSystem.Api.csproj"

# Copiar o restante dos arquivos e compilar o projeto
COPY . .
WORKDIR /app/src/OrderSystem.Api
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar os binários publicados do estágio de build
COPY --from=build /app/publish .

EXPOSE 5001

# Definir o ponto de entrada da aplicação
ENTRYPOINT ["dotnet", "OrderSystem.Api.dll"]
