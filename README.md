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

### Load environment variables:

```cmd 
load-env.cmd .\src\CABasicCRUD.Host.Sqlite.WebApi\.env  
```

### Run locally

Apply migrations:

```cmd
dotnet ef database update --project .\src\CABasicCRUD.Infrastructure.Persistence.Sqlite --startup-project .\src\CABasicCRUD.Host.Sqlite.WebApi
```
For connection string `Data Source=data.db`, the database file is created inside the startup project directory.

Run project:

```cmd
dotnet run --project src\CABasicCRUD.Host.Sqlite.WebApi
```
### Publish and run locally

Publish and run publish artifact locally:

Create migrations bundle:

```cmd
dotnet ef migrations bundle --project .\src\CABasicCRUD.Host.Sqlite.WebApi
```

Run migrations bundle:

```cmd
efbundle.exe
```

This creates the database file at the project root for `Data Source=data.db`

```cmd
dotnet publish .\src\CABasicCRUD.Host.Sqlite.WebApi -c Release -o .\publish
dotnet .\publish\CABasicCRUD.Host.Sqlite.WebApi.dll
```


