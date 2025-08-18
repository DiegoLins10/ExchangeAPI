# 🚀 **Exchange.API — API de Câmbio em Tempo Real**

## 📖 Descrição

API RESTful desenvolvida em .NET para conversão de moedas em tempo real, construída com **Clean Architecture** e **Domain-Driven Design (DDD)**. Utiliza **Use Cases** para garantir o isolamento das regras de negócio e o desacoplamento total entre as camadas, facilitando manutenção, testes e escalabilidade.

A API suporta:

* 💱 Conversão de valores entre diferentes moedas usando taxas de câmbio do Banco central brasileiro.
* 📜 Histórico de conversões realizadas para consulta.
* 🔧 Extensibilidade para futuras integrações com  APIs de câmbio de outras instituições, agendamento de conversões e notificações.

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
├── Exchange.Infrastructure/     # Implementações dos repositórios, serviços externos e persistência
│    ├── Repositories/
│    └── Services/
└── Exchange.Unit.Test/     # Implementações dos testes unitarios
     ├── Application/
     └── API/
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
  "toCurrency": "EUR",
  "amountBRL": 1000,
  "dateQuotation": "2025-08-13",
  "exchangeType": 1
}
```

---

## 📈 Exemplo de Resposta

```json
{
    "originalAmount": 1000,
    "fromCurrency": "BRL",
    "convertedAmount": 158.75,
    "toCurrency": "EUR",
    "exchangeRate": 6.30
}
```


---

## ⚠️ Tratamento de Erros com Middleware global

* 🚫 Valores inválidos (ex.: argumentos incorretos) resultam em resposta HTTP **400 Bad Request**, com mensagens claras para facilitar o entendimento do problema.
* ❌ Erros inesperados ou internos são capturados globalmente por um **middleware de tratamento de exceções**, que garante o retorno de uma resposta HTTP **500 Internal Server Error** padronizada e evita vazamento de detalhes sensíveis.
* 💡 Esse middleware centraliza o tratamento de erros, simplificando o código dos controllers e melhorando a manutenção da aplicação.

---


### 🚀 Próximos Passos

* (✅ DONE) 🔗 **Integrar API oficial do Banco Central do Brasil (Bacen)** para obter taxas de câmbio oficiais e atualizadas.  
Fonte:
[Bacen - Taxas de Câmbio - Dados Abertos](https://dadosabertos.bcb.gov.br/dataset/taxas-de-cambio-todos-os-boletins-diarios/resource/61318ccb-db9d-4d6c-87f5-d8013af7a401?inner_span=True)  
Exemplo: [Bacen - Exemplo de busca](https://olinda.bcb.gov.br/olinda/servico/PTAX/versao/v1/aplicacao#!/recursos/CotacaoMoedaDia#eyJmb3JtdWxhcmlvIjp7IiRmb3JtYXQiOiJqc29uIiwiJHRvcCI6MSwibW9lZGEiOiJFVVIiLCJkYXRhQ290YWNhbyI6IjA4LTE1LTIwMjUifSwicGVzcXVpc2FkbyI6dHJ1ZSwiYWN0aXZlVGFiIjoiZGFkb3MiLCJncmlkU3RhdGUiOnsDMAM6W3sDQgMiBDAEIiwDQQN9LHsDQgMiBDEEIiwDQQN9LHsDQgMiBDIEIiwDQQN9LHsDQgMiBDMEIiwDQQN9LHsDQgMiBDQEIiwDQQN9LHsDQgMiBDUEIiwDQQN9XSwDMQM6e30sAzIDOltdLAMzAzp7fSwDNAM6e30sAzUDOnt9fSwicGl2b3RPcHRpb25zIjp7A2EDOnt9LANiAzpbXSwDYwM6NTAwLANkAzpbXSwDZQM6W10sA2YDOltdLANnAzoia2V5X2FfdG9feiIsA2gDOiJrZXlfYV90b196IiwDaQM6e30sA2oDOnt9LANrAzo4NSwDbAM6ZmFsc2UsA20DOnt9LANuAzp7fSwDbwM6IkNvbnRhZ2VtIiwDcAM6IlRhYmxlIn19)
* ⏰ Adicionar agendamento de conversões com notificação quando taxa atingir determinado valor.
* 🔐 Implementar autenticação e autorização.
* (✅ DONE) Adicionar testes automatizados.

---


