# 🏍️ Rental API

API para gerenciamento de **aluguel de motos e entregadores**, com suporte a mensageria via **RabbitMQ** e persistência em **Postgres**.

---

## 🚀 Tecnologias

- **.NET 8**
- **Entity Framework Core** (Postgres)
- **RabbitMQ** (mensageria de eventos)
- **Docker & Docker Compose**
- **xUnit + Testcontainers** (testes automatizados)

---

## ⚙️ Pré-requisitos

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado e rodando.
- .NET SDK 8+ instalado.
- [pgAdmin](https://www.pgadmin.org/).

---

## 🐳 Subindo dependências (Postgres + RabbitMQ)

O projeto inclui um `docker-compose.yml` na raiz para orquestrar os serviços necessários.

### Subir containers:
```bash
docker-compose up -d
```

## Aplicar migrations
dotnet ef database update -p Infra -s WsRental

## 📬 Mensageria (RabbitMQ)

A API publica eventos quando uma moto é cadastrada.
Um consumer roda em background e persiste no banco apenas motos do ano 2024.

## Fluxo
 - MotorcyclesController → cadastra moto.
 - MotorcycleService → publica evento no RabbitMQ (motorcycle-registered).
 - MotorcycleConsumer → consome mensagem.
 - Se Year == 2024, persiste no Postgres.

## Acessar o RabbitMQ Management UI
Após subir os containers, o RabbitMQ pode ser acessado via navegador:
 - URL: http://localhost:15672
 - Usuário: guest
 - Senha: guest

## 🧪 Testes Automatizados
O projeto usa xUnit + Testcontainers para rodar testes de integração em containers.

O fluxo dos testes:
 - Sobe um container Postgres isolado.
 - Aplica migrations automaticamente.
 - Executa os testes de API/Controllers.
 - Container é destruído ao final.

## 🏗️ Arquitetura e Padrões de Projeto
O sistema segue uma arquitetura em camadas inspirada em Clean Architecture e DDD (Domain-Driven Design):
 - Domain → Entidades puras, encapsulam estado e comportamento.
 - Infrastructure → Acesso a dados com EF Core, aplicando Repository Pattern e atuando como Facade sobre a persistência.
 - Services → Regras de negócio e mensageria (publicação/consumo de eventos via RabbitMQ). Aplica Mediator Pattern (orquestra chamadas) e Observer Pattern (consumers reagem a eventos).
 - WsRental (API) → Camada de apresentação (Controllers), implementa MVC Pattern e usa Dependency Injection para desacoplamento.
 - Tests → Unit e Integration Tests com xUnit e Testcontainers, aplicando Builder Pattern (containers) e Factory Method (fixtures).

## Fluxo simplificado
Controller → Service → Repository/DbContext → RabbitMQ → Consumer → Persistência
Essa abordagem garante:
 - Separação de responsabilidades
 - Facilidade de manutenção
 - Testabilidade
 - Escalabilidade

## 📂 Estrutura do Projeto
 - /Domain                 -> Entidades de negócio
 - /Infrastructure         -> DbContext, Repositórios, Migrations
 - /Services               -> Regras de negócio, Mensageria (Publisher/Consumer)
 - /WsRental               -> API (Controllers, Program.cs)
 - /Tests                  -> Testes unitários e de integração (xUnit + Testcontainers)

## Diferenciais 🚀
- Testes unitários
- Testes de integração
- EntityFramework
- Docker e Docker Compose
- Design Patterns
- Documentação
- Tratamento de erros
- Arquitetura e modelagem de dados
- Código escrito em língua inglesa
- Código limpo e organizado
- Logs bem estruturados
- Seguir convenções utilizadas pela comunidade