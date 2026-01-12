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
~~For connection string `Data Source=data.db`, the database file is created inside the startup project directory.~~
Avoid relative file paths for database. Use absolute paths.


Run project:

```cmd
dotnet run --project src\CABasicCRUD.Host.Sqlite.WebApi
```
### Publish and run locally

Create migrations bundle:

```cmd
dotnet ef migrations bundle --project .\src\CABasicCRUD.Host.Sqlite.WebApi
```

Run migrations bundle:

```cmd
efbundle.exe
```

~~This creates the database file at the project root for `Data Source=data.db`~~ Use absolute file path.

Publish and run publish artifact locally:

```cmd
dotnet publish .\src\CABasicCRUD.Host.Sqlite.WebApi -c Release -o .\publish
```

When running from the root path, set DOTNET_CONTENTROOT environment variable to the absolute path of the folder containing the required appsettings.json configuration, which in our case is the publish directory.

```cmd
dotnet .\publish\CABasicCRUD.Host.Sqlite.WebApi.dll
```

Otherwise

```cmd
cd publish
dotnet CABasicCRUD.Host.Sqlite.WebApi.dll
```

### Run using docker

After loading environment variables, run the following command from the root of the project:

```cmd
docker compose -f .\src\CABasicCRUD.Host.Sqlite.WebApi\docker-compose.yml up --build -d
```

To stop the app:

```cmd
docker compose -f .\src\CABasicCRUD.Host.Sqlite.WebApi\docker-compose.yml down
```




## 2. Host.PostgreSql.WebApi

Projects:
- Infrastructure.Persistence.PostgreSql
- Presentation.WebApi

### Load environment variables:

```cmd 
load-env.cmd .\src\CABasicCRUD.Host.PostgreSql.WebApi\.env  
```

### Run locally

Apply migrations:

```cmd
dotnet ef database update --project .\src\CABasicCRUD.Infrastructure.Persistence.PostgreSql --startup-project .\src\CABasicCRUD.Host.PostgreSql.WebApi
```

Run project:

```cmd
dotnet run --project src\CABasicCRUD.Host.PostgreSql.WebApi
```
### Publish and run locally

Create migrations bundle:

```cmd
dotnet ef migrations bundle --project .\src\CABasicCRUD.Host.PostgreSql.WebApi
```

Run migrations bundle:

```cmd
efbundle.exe
```

Publish and run publish artifact locally:

```cmd
dotnet publish .\src\CABasicCRUD.Host.PostgreSql.WebApi -c Release -o .\publish
```

When running from the root path, set DOTNET_CONTENTROOT environment variable to the absolute path of the folder containing the required appsettings.json configuration, which in our case is the publish directory.

```cmd
dotnet .\publish\CABasicCRUD.Host.PostgreSql.WebApi.dll
```

Otherwise

```cmd
cd publish
dotnet CABasicCRUD.Host.PostgreSql.WebApi.dll
```

### Run using docker

After loading environment variables, run the following command from the root of the project:

```cmd
docker compose -f .\src\CABasicCRUD.Host.PostgreSql.WebApi\docker-compose.yml up --build -d
```

To stop the app:

```cmd
docker compose -f .\src\CABasicCRUD.Host.PostgreSql.WebApi\docker-compose.yml down
```