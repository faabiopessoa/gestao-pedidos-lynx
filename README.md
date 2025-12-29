# Gestão de Pedidos Lynx

A aplicação implementa um fluxo simples de gestão de pedidos, com backend em ASP.NET Core e frontend em Angular.

## Arquitetura

- Backend: ASP.NET e EF Core
- DB: SQLite
- Frontend: Angular

## Backend

### Rodar

```bash
cd backend
dotnet restore
dotnet run
```

## Banco de Dados

- SQLite
- Migrations versionadas em `backend/Migratios`

## Endpoints

- GET `/products`
- GET `/orders`
- GET `/orders/{id}`
- POST `/orders`
- POST `/payments`

## Frontend

### Rodar

```bash
cd frontend
npm i
npm start
```

### Funcionalidades

- Tabela de produtos com filtro
- Carrinho local com finalização de pedido
- Lista de pedidos com aba de detalhes do pedido

**_Projeto desenvolvido por Fábio Pessoa_**
