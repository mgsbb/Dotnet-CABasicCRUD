# Environment variables

Use **load-env.cmd** script to load environment variables from the specified .env file into the current executing shell. Make sure to set the values in the .env file.

Example:

```cmd
load-env.cmd src\CABasicCRUD.Host.Sqlite.WebApi\.env
```

# Composition Roots

Domain, Application and Infrastructure are projects common to all hosts. Make sure the correct host environment variables are loaded into the current executing shell.

## 1. Host.Sqlite.WebApi

Projects:
- Infrastructure.Persistence.Sqlite
- Presentation.WebApi

Load environment variables:

```cmd 
load-env.cmd .\src\CABasicCRUD.Host.Sqlite.WebApi\.env  
```

Apply migrations:

```cmd
dotnet ef database update --project .\src\CABasicCRUD.Infrastructure.Persistence.Sqlite --startup-project .\src\CABasicCRUD.Host.Sqlite.WebApi
```
For connection string `Data Source=data.db`, the database file is created inside the startup project directory.

Run project locally:

```cmd
dotnet run --project src\CABasicCRUD.Host.Sqlite.WebApi
```

Publish and run publish artifact locally:

```cmd
dotnet publish .\src\CABasicCRUD.Host.Sqlite.WebApi -c Release -o .\publish
move .\src\CABasicCRUD.Host.Sqlite.WebApi\data.db .
dotnet .\publish\CABasicCRUD.Host.Sqlite.WebApi.dll
```

`data.db` file moved from src folder to root, so that the publish artifact can access it.

