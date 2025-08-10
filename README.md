# 🚀 **Exchange.API — API de Câmbio em Tempo Real**

## 📖 Descrição

API RESTful desenvolvida em .NET para conversão de moedas em tempo real, construída com **Clean Architecture** e **Domain-Driven Design (DDD)**. Utiliza **Use Cases** para garantir o isolamento das regras de negócio e o desacoplamento total entre as camadas, facilitando manutenção, testes e escalabilidade.

A API suporta:

* 💱 Conversão de valores entre diferentes moedas usando taxas de câmbio simuladas (fictícias).
* 📜 Histórico de conversões realizadas para consulta.
* 🔧 Extensibilidade para futuras integrações com APIs reais de câmbio, agendamento de conversões e notificações.

---

## 📁 Estrutura do Projeto

```
Exchange.sln
├── Exchange.API/                # API REST, controllers, middleware e configuração
│   ├── Controllers/
│   ├── Middleware/
│   └── Program.cs
├── Exchange.Application/        # Casos de uso, interfaces e lógica de aplicação
│   ├── Interfaces/
│   └── UseCases/
├── Exchange.Domain/             # Entidades do domínio, interfaces e regras de negócio puras
│   ├── Entities/
│   └── Interfaces/
└── Exchange.Infrastructure/     # Implementações dos repositórios, serviços externos e persistência
    ├── Repositories/
    └── Services/
```

---

## 🛠️ Tecnologias Utilizadas

* .NET 8
* Clean Architecture
* Domain-Driven Design (DDD)
* ASP.NET Core Web API
* Injeção de Dependência
* Middlewares para tratamento global de erros

---

## ▶️ Como Rodar

1. 🔽 Clone o repositório
2. 🛠️ Abra a solução `Exchange.sln` no Visual Studio ou VS Code
3. 📦 Restaure as dependências e compile o projeto
4. ▶️ Execute o projeto `Exchange.API`
5. 🌐 Acesse a documentação Swagger em `https://localhost:{porta}/swagger` (se configurado)
6. 💸 Use o endpoint `POST /api/currency/convert` para realizar conversões

---

## 🔍 Endpoints Principais

| Método | Endpoint                | Descrição                                     |
| ------ | ----------------------- | --------------------------------------------- |
| POST   | `/api/currency/convert` | Converte um valor de uma moeda para outra.    |
| GET    | `/api/currency/history` | Retorna o histórico de conversões realizadas. |

---

## 📄 Exemplo de Requisição

```json
POST /api/currency/convert
{
  "fromCurrency": "USD",
  "toCurrency": "BRL",
  "amount": 100.0
}
```

---

## 📈 Exemplo de Resposta

```json
{
  "originalAmount": 100.0,
  "fromCurrency": "USD",
  "convertedAmount": 520.0,
  "toCurrency": "BRL",
  "exchangeRate": 5.2
}
```


---

## ⚠️ Tratamento de Erros com Middleware global

* 🚫 Valores inválidos (ex.: argumentos incorretos) resultam em resposta HTTP **400 Bad Request**, com mensagens claras para facilitar o entendimento do problema.
* ❌ Erros inesperados ou internos são capturados globalmente por um **middleware de tratamento de exceções**, que garante o retorno de uma resposta HTTP **500 Internal Server Error** padronizada e evita vazamento de detalhes sensíveis.
* 💡 Esse middleware centraliza o tratamento de erros, simplificando o código dos controllers e melhorando a manutenção da aplicação.

---


### 🚀 Próximos Passos

* 🔗 **Integrar API oficial do Banco Central do Brasil (Bacen)** para obter taxas de câmbio oficiais e atualizadas.  
Fonte:
[Bacen - Taxas de Câmbio - Dados Abertos](https://dadosabertos.bcb.gov.br/dataset/taxas-de-cambio-todos-os-boletins-diarios/resource/61318ccb-db9d-4d6c-87f5-d8013af7a401?inner_span=True)
* ⏰ Adicionar agendamento de conversões com notificação quando taxa atingir determinado valor.
* 🔐 Implementar autenticação e autorização.
* ✅ Adicionar testes automatizados.

---


