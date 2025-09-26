## Docker file explicação

### **Etapa 1: Build**

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
```

* **O que faz:** Define a imagem base para construir o projeto.
* `mcr.microsoft.com/dotnet/sdk:8.0` é a imagem oficial da Microsoft com o SDK do .NET 8, que já tem tudo que você precisa para compilar seu código.
* `AS build` dá um nome para essa etapa (“build”), porque depois você vai copiar os arquivos compilados para outra etapa.

```dockerfile
WORKDIR /src
```

* **O que faz:** Define o diretório de trabalho dentro do container.
* Todo comando seguinte (`COPY`, `RUN`) será executado dentro de `/src`.

```dockerfile
COPY ["Exchange.API/Exchange.API.csproj", "Exchange.API/"]
COPY ["Exchange.Application/Exchange.Application.csproj", "Exchange.Application/"]
COPY ["Exchange.Domain/Exchange.Domain.csproj", "Exchange.Domain/"]
COPY ["Exchange.Infrastructure/Exchange.Infrastructure.csproj", "Exchange.Infrastructure/"]
```

* **O que faz:** Copia apenas os arquivos `.csproj` de cada projeto para dentro do container.
* Isso é feito **antes de copiar todo o código** para aproveitar o cache do Docker e acelerar builds futuros.

```dockerfile
RUN dotnet restore "Exchange.API/Exchange.API.csproj"
```

* **O que faz:** Restaura todas as dependências do projeto, baixando pacotes NuGet.
* Isso acontece só com os `.csproj`, o que é mais rápido do que restaurar após copiar todo o código.

```dockerfile
COPY . .
```

* **O que faz:** Copia todo o restante do código-fonte para dentro do container.

```dockerfile
WORKDIR "/src/Exchange.API"
```

* **O que faz:** Muda o diretório de trabalho para a pasta do projeto API, que será compilado.

```dockerfile
RUN dotnet publish -c Release -o /app/publish
```

* **O que faz:** Compila e publica o projeto em **modo Release**.
* `-o /app/publish` define a pasta de saída, que será usada na etapa de runtime.

---

### **Etapa 2: Runtime**

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
```

* **O que faz:** Define a imagem base final para rodar a aplicação.
* Essa imagem é **menor** que o SDK, porque só precisa do runtime do .NET, não das ferramentas de compilação.

```dockerfile
WORKDIR /app
```

* Define a pasta onde a aplicação vai “viver” dentro do container.

```dockerfile
COPY --from=build /app/publish .
```

* **O que faz:** Copia os arquivos que foram publicados na etapa de build para o diretório atual do container.
* `--from=build` indica que estamos pegando arquivos da etapa anterior chamada `build`.

```dockerfile
EXPOSE 80
```

* **O que faz:** Expõe a porta 80 para que o container possa receber tráfego HTTP.
* Não abre automaticamente a porta, mas informa que o container usa essa porta.

```dockerfile
ENTRYPOINT ["dotnet", "Exchange.API.dll"]
```

* **O que faz:** Define o comando que será executado quando o container iniciar.
* Aqui, ele inicia a aplicação .NET.

---

✅ **Resumo geral:**

1. **Build stage:** compila seu código e publica os arquivos prontos.
2. **Runtime stage:** pega os arquivos compilados e cria um container enxuto só para rodar a aplicação.

Essa abordagem é chamada de **multi-stage build**, porque você separa compilação e runtime, deixando a imagem final mais leve.

---
