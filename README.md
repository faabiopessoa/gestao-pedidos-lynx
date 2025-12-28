# Gestão de Pedidos Lynx

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
- GET `/payments`