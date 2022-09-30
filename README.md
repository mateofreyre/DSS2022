# DSS 2022

Trello board: 


## Techonology Stack
- .Net Core + Entity Framework
- React

### Side frameworks and technologies


## Database
To start the database server, run:
```bash
docker compose -f docker-compose-db.yml up
```

## Backend

### Requirements

1. .Net 6.0

Run in development mode: 
```bash
cd backend/SoliSYSTEMS/Max.Core.Api
dotnet watch run
```

**The backend application runs on port 5000**
### Migrations
The Data Layer is built with Entity Framework so we need to create and run migrations to keep the database structure up to date with the application entity model.

Create migration:
```bash
cd DSS2022.Api
dotnet ef migrations add <MigrationName>
```

Update the database:
```bash
cd DSS2022.Api
dotnet ef database update
```

Initial Login:
The first user created is 'admin@solisystems.com'. This user is inactive by default. 
To activate the user, you must do the next steps.
1. Run the backend.
2. Make a PUT in Insomnia or Postman with the next url: <url_backend>/api/users/activate with the next JSON:
```json
    {"token": "TOKEN", "password": "Test123!"}
```