# 📄 PropostaService – API de Gestão de Propostas de Seguro

Este repositório contém a implementação de um sistema para gerenciamento e contratação de propostas de seguro, desenvolvido como parte de um teste técnico, seguindo a Arquitetura Hexagonal (Ports & Adapters) e boas práticas de Clean Code, DDD e SOLID.
---

## 🚀 Tecnologias Utilizadas

- .NET com C#
- SQLite como banco de dados relacional
- Docker para conteinerização
- RabbitMQ para comunicação assíncrona
- Swagger para documentação da API
- AutoMapper para mapeamento de objetos
- XUnit para testes unitários
- Logger para rastreamento e auditoria

---

## 🧩 Arquitetura

- **Hexagonal Architecture (Ports & Adapters)**: separação clara entre domínio, aplicação e infraestrutura.
- **Microserviços**: este serviço é parte de uma plataforma maior, podendo se comunicar com outros serviços via filas (RabbitMQ) ou APIs REST.
- **DDD, SOLID e Design Patterns**: aplicados para garantir escalabilidade, manutenção e legibilidade do código.

---

## 📚 Funcionalidades

- Criar proposta de seguro
- Listar propostas existentes
- Alterar status da proposta: `Em Análise`, `Aprovada`, `Rejeitada`
- Consultar proposta por ID
- Contratar proposta aprovada

---

## 🔐 Autenticação

> Este serviço não implementa autenticação por padrão, mas pode ser facilmente integrado com JWT ou OAuth2.

---

## 📦 Instalação e Execução

### Pré-requisitos

- Docker instalado
- .NET SDK (caso deseje rodar localmente sem Docker)
- .RabbitMQ rodando localmente

### Executando com Docker

```bash
docker-compose up --build