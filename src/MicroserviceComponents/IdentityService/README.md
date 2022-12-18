# CleanMicroserviceSystem.Themis.WebAPI

## Generate Database Migrations code file

1. Select `CleanMicroserviceSystem.Themis.WebAPI` or your specified project as Startup project;
2. Open `Package Manager Console` in Visual Studio;
3. Select `CleanMicroserviceSystem.Themis.Infrastructure` as Default project in Package Manager Console;
4. Input below commands and execute;

### 

## DbContexts

- Identity
  - ThemisDBContext
- IdentityServer
  - ConfigurationDbContext
  - PersistedGrantDbContext

```
## To remove existed mrigration ode files
remove-migration -Context PersistedGrantDbContext
remove-migration -Context ConfigurationDbContext
remove-migration -Context IdentityDbContext
remove-migration -Context ThemisDBContext

## To generate database migration code files automatically
add-migration InitialMigration -Context ThemisDBContext
add-migration MigrateIdentityDbContext -Context IdentityDbContext
add-migration MigrateConfigurationDbContext -Context ConfigurationDbContext
add-migration MigratePersistedGrantDbContext -Context PersistedGrantDbContext

## To execute database migration code files and apply modifications to current connected database file
Update-Database
```
