# CleanMicroserviceSystem.Astra.WebAPI

## Packages

### Publish

```
dotnet nuget push -s http://10.1.100.73:21003 .\automapper.12.0.1.nupkg -k ASTRA_NUGET_SERVER_API_KEY_PRODUCTION
```

## Generate Database Migrations code file

1. Select `CleanMicroserviceSystem.Astra.WebAPI` or your specified project as Startup project;
2. Open `Package Manager Console` in Visual Studio;
3. Select `CleanMicroserviceSystem.Astra.Infrastructure` as Default project in Package Manager Console;
4. Input below commands and execute;

## DbContexts

- Application
  
  - AstraDBContext

- NuGet
  
  - BaGetDBContext

```
## To remove existed mrigration ode files
remove-migration -Context AstraDBContext
remove-migration -Context BaGetDBContext

## To generate database migration code files automatically
add-migration InitialMigration -Context AstraDBContext
add-migration MigrateBaGetDBContext -Context BaGetDBContext

## To execute database migration code files and apply modifications to current connected database file
Update-Database -Context AstraDBContext
Update-Database -Context BaGetDBContext
```
