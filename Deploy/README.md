# 🚀 Deploy de API no ECS com Fargate + ECR

Este guia documenta o processo para **build, push e deploy** de uma aplicação .NET/Java/Python (qualquer API) no **Amazon ECS com Fargate**, usando imagem armazenada no **Amazon ECR**.
Além do passo a passo, contém **dicas e armadilhas que enfrentei** para evitar dores futuras.

---

## 📌 Pré-requisitos

* AWS CLI configurada (`aws configure`)
* Docker instalado (para build e push da imagem)
* Permissões na conta AWS para ECR + ECS

---

## 🔨 1. Login no ECR

```sh
aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin <AWS_ACCOUNT_ID>.dkr.ecr.us-east-1.amazonaws.com
```

---

## 📦 2. Criar repositório no ECR

```sh
aws ecr create-repository --repository-name exchange-api
```

---

## 🏗️ 3. Build e push da imagem

```sh
# Build local
docker build -t exchange-api -f Exchange.API/Dockerfile .

# Tag da imagem
docker tag exchange-api:latest <AWS_ACCOUNT_ID>.dkr.ecr.us-east-1.amazonaws.com/exchange-api:latest

# Push para o ECR
docker push <AWS_ACCOUNT_ID>.dkr.ecr.us-east-1.amazonaws.com/exchange-api:latest
```

---

## ☁️ 4. Criar cluster ECS (Console AWS)

1. Acesse **ECS → Clusters → Create Cluster**
2. Escolha **Fargate (Networking only)**
3. Nome: `exchange-cluster`
4. Selecione **VPC e Subnets**
5. **Criar**

---

## 📑 5. Criar Task Definition (Console AWS)

1. ECS → **Task definitions → Create new**
2. Tipo: **Fargate**
3. CPU/Memória (ex.: 0.25 vCPU, 512MB)
4. Adicione container:

   * Nome: `exchange-api`
   * Imagem: URL do ECR (`<AWS_ID>.dkr.ecr.us-east-1.amazonaws.com/exchange-api:latest`)
   * Porta: `8080` (se sua aplicação escuta em 8080)
5. Salvar

---

## ⚙️ 6. Criar Service (Console AWS)

1. ECS → **Cluster → Create → Service**
2. Launch type: **Fargate**
3. Task definition: escolha a criada
4. Service name: `exchange-service`
5. Number of tasks: `1`
6. Networking:

   * VPC: a mesma do cluster
   * Subnets: públicas (pra teste)
   * Security Group: **⚠️ configurar inbound e outbound**

     * Inbound: permitir porta **80** (para ALB ou acesso direto)
     * Inbound: permitir porta **8080** se ALB estiver escutando nessa porta
     * Outbound: liberar acesso **0.0.0.0/0** (para saída geral)
7. Load Balancer:

   * (Opcional) ALB → Listener 80 → Target Group (porta 8080 do container)

---

## ✅ 7. Testar

* Se usou ALB:

  ```http
  http://alb-exchange-1526545477.us-east-1.elb.amazonaws.com
  ```
* Se expôs IP público na Task:

  ```http
  http://<Public-IP>
  ```

---

## ⚠️ Dores e Armadilhas que sofri

* **SG Inbound/Outbound:** se esquecer de liberar porta 8080 (container) e 80 (ALB), o serviço **não sobe**.
* **Mesma SG para ALB e ECS:** causa confusão porque precisa liberar ambas as portas no mesmo grupo → **boa prática é separar** SGs.
* **EXPOSE no Dockerfile:** não abre porta, só documenta. O que vale é o `containerPort` da task.
* **Confundir porta do ALB e porta do container:** não precisam ser iguais. ALB (80/443) pode redirecionar para qualquer porta interna (ex.: 8080).
* **Health Check do ALB:** precisa estar configurado para a porta do container (8080), senão a task fica **Unhealthy**.

---

## 📝 Checklist rápido antes do deploy

* [ ] Imagem criada e publicada no ECR
* [ ] Porta certa definida no `containerPort` da task
* [ ] SG do ALB libera **80/443** (entrada pública)
* [ ] SG do ECS libera **8080** (entrada do ALB)
* [ ] Outbound do ECS liberado para internet (quando precisa chamar serviços externos)

---

## 🔗 Fluxo da Arquitetura

```
Cliente (Browser / Postman)
           │
           ▼
   ┌────────────────────┐
   │  ALB (porta 80/443)│
   └─────────┬──────────┘
             │ encaminha
             ▼
   ┌────────────────────┐
   │ Target Group       │
   │ (porta 8080)       │
   └─────────┬──────────┘
             │
             ▼
   ┌────────────────────┐
   │ ECS Task (Fargate) │
   │ Container escutando│
   │ porta 8080         │
   └────────────────────┘
```

---

## 🔐 Security Groups

* **SG do ALB**

  * Inbound: 80/443 de `0.0.0.0/0` (internet)
  * Outbound: 8080 para o **SG do ECS**

* **SG do ECS**

  * Inbound: 8080 apenas do **SG do ALB**
  * Outbound: `0.0.0.0/0` (internet, se necessário)

---

## 💰 Checklist de custos ECS + ALB

### 1️⃣ ECS Cluster

* **Custo:** ❌ nenhum
* **Observação:** É só um agrupamento lógico, vazio não cobra nada.

### 2️⃣ ECS Task (Fargate)

* **Custo:** ✅ cobra por vCPU e memória por hora enquanto estiver rodando.
* **Dica:** Se for teste, **pare ou delete a task** quando não estiver usando.

### 3️⃣ ECS Service

* **Custo:** ✅ cobra se estiver mantendo Tasks ativas.
* **Dica:** Delete o service se não precisar de scaling automático ou tasks rodando.

### 4️⃣ Load Balancer (ALB/NLB)

* **Custo:** ✅ cobra **por hora** + **LCU (Load Balancer Capacity Unit)**.
* **Dica:** Se só era pra teste, delete o ALB e o target group quando não precisar.

### 5️⃣ Target Groups

* **Custo:** ❌ não gera custo direto, mas se estiver ligado ao ALB, o ALB sim.

### 6️⃣ Subnets / Security Groups

* **Custo:** ❌ normalmente gratuito (a não ser que haja NAT Gateway ligado).

### 7️⃣ NAT Gateway

* **Custo:** ✅ cobra por hora + GB transferido.
* **Dica:** Não é necessário se sua API está pública com ALB.

### 8️⃣ Imagens no ECR

* **Custo:** ✅ cobra por GB armazenado.
* **Dica:** Delete imagens antigas que não vai usar.

### 9️⃣ CloudWatch / Logs

* **Custo:** ✅ cobra por GB armazenado + métricas custom.
* **Dica:** Configure retenção curta pra logs de teste.

---

💡 **Resumo:**
Se você só criou o **cluster** sem tasks ou ALB ativo → zero custo.
Se houver **tasks Fargate, ALB ou NAT Gateway** → começa a gerar custos.



