# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os csproj e restaura dependências
COPY ["Exchange.API/Exchange.API.csproj", "Exchange.API/"]
COPY ["Exchange.Application/Exchange.Application.csproj", "Exchange.Application/"]
COPY ["Exchange.Domain/Exchange.Domain.csproj", "Exchange.Domain/"]
COPY ["Exchange.Infrastructure/Exchange.Infrastructure.csproj", "Exchange.Infrastructure/"]

RUN dotnet restore "Exchange.API/Exchange.API.csproj"

# Copia todo o código e faz o build
COPY . .
WORKDIR "/src/Exchange.API"
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expõe a porta dentro do container
EXPOSE 8080

ENTRYPOINT ["dotnet", "Exchange.API.dll"]
