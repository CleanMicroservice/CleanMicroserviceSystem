# CleanMicroserviceSystem

<p align="center">
   <img src="https://raw.github.com/FinancialMarketSimulator/CleanMicroserviceSystem/master/doc/Logo.png" align="center"/>
   <h2 align="center">Clean Microservice System</h2>
   <p align="center">A Microservice System template repository based on Clean Architecture and ASP.NET Core Blazor Web Assembly.</p>
</p>

## Status

<p align="center">
   <a href="https://github.com/FinancialMarketSimulator/CleanMicroserviceSystem/actions/workflows/dotnet.yml">
      <img border="0" src="https://github.com/FinancialMarketSimulator/CleanMicroserviceSystem/workflows/.Net%20Build/badge.svg" />
   </a>
   <a href="https://github.com/FinancialMarketSimulator/CleanMicroserviceSystem/blob/master/LICENSE">
      <img border="0" src="https://img.shields.io/github/license/FinancialMarketSimulator/CleanMicroserviceSystem" />
   </a>
   <a href="https://github.com/FinancialMarketSimulator/CleanMicroserviceSystem/search?l=c%23">
      <img border="0" src="https://img.shields.io/github/languages/top/FinancialMarketSimulator/CleanMicroserviceSystem" />
   </a>
   <a href="https://github.com/FinancialMarketSimulator/CleanMicroserviceSystem">
      <img border="0" src="https://img.shields.io/github/directory-file-count/FinancialMarketSimulator/CleanMicroserviceSystem" />
   </a>
   <a href="https://github.com/FinancialMarketSimulator/CleanMicroserviceSystem/archive/refs/heads/master.zip">
      <img border="0" src="https://img.shields.io/github/repo-size/FinancialMarketSimulator/CleanMicroserviceSystem" />
   </a>
   <a href="https://github.com/FinancialMarketSimulator/CleanMicroserviceSystem/issues?q=is%3Aopen+is%3Aissue">
      <img border="0" src="https://img.shields.io/github/issues/FinancialMarketSimulator/CleanMicroserviceSystem" />
   </a>
   <a href="https://github.com/FinancialMarketSimulator/CleanMicroserviceSystem/network/members">
      <img border="0" src="https://img.shields.io/github/forks/FinancialMarketSimulator/CleanMicroserviceSystem" />
   </a>
   <a href="https://github.com/FinancialMarketSimulator/CleanMicroserviceSystem/stargazers">
      <img border="0" src="https://img.shields.io/github/stars/FinancialMarketSimulator/CleanMicroserviceSystem" />
   </a>
   <a href="https://github.com/FinancialMarketSimulator/CleanMicroserviceSystem/watchers">
      <img border="0" src="https://img.shields.io/github/watchers/FinancialMarketSimulator/CleanMicroserviceSystem" />
   </a>
   <a href="https://github.com/FinancialMarketSimulator/CleanMicroserviceSystem/releases">
      <img border="0" src="https://img.shields.io/github/v/release/FinancialMarketSimulator/CleanMicroserviceSystem?include_prereleases" />
   </a>
   <a href="https://github.com/FinancialMarketSimulator/CleanMicroserviceSystem/releases">
      <img border="0" src="https://img.shields.io/github/release-date-pre/FinancialMarketSimulator/CleanMicroserviceSystem" />
   </a>
   <a href="https://github.com/FinancialMarketSimulator/CleanMicroserviceSystem/archive/refs/heads/master.zip">
      <img border="0" src="https://img.shields.io/github/downloads/FinancialMarketSimulator/CleanMicroserviceSystem/total" />
   </a>
   <a href="https://github.com/FinancialMarketSimulator/CleanMicroserviceSystem/tags">
      <img border="0" src="https://img.shields.io/github/v/tag/FinancialMarketSimulator/CleanMicroserviceSystem" />
   </a>
   <a href="https://github.com/FinancialMarketSimulator/CleanMicroserviceSystem/releases">
      <img border="0" src="https://img.shields.io/github/commits-since/FinancialMarketSimulator/CleanMicroserviceSystem/latest/master?include_prereleases" />
   </a>
   <a href="https://github.com/FinancialMarketSimulator/CleanMicroserviceSystem/commits/master">
      <img border="0" src="https://img.shields.io/github/last-commit/FinancialMarketSimulator/CleanMicroserviceSystem/master" />
   </a>
</p>

## Nuget Packages

<p align="center">
   <a href="https://www.nuget.org/packages/CleanMicroserviceSystem.PlaceHolder/">
      <img border="0" src="https://img.shields.io/nuget/vpre/CleanMicroserviceSystem.PlaceHolder?label=CleanMicroserviceSystem.PlaceHolder&style=flat-square" />
   </a>
</p>
## Generate Database Migrations code file

1. Select `CleanMicroserviceSystem.Tethys.WebAPI` or your specified project as Startup project;
2. Open `Package Manager Console` in Visual Studio;
3. Select `CleanMicroserviceSystem.Tethys.Infrastructure` as Default project in Package Manager Console;
4. Input below commands and execute;

### Commands

| Command                  | Description                                                  |
| ------------------------ | ------------------------------------------------------------ |
| Get-Help entityframework | Displays information about entity framework commands.        |
| Add-Migration            | Creates a migration by adding a migration snapshot.          |
| Remove-Migration         | Removes the last migration snapshot.                         |
| Update-Database          | Updates the database schema based on the last migration snapshot. |
| Script-Migration         | Generates a SQL script using all the migration snapshots.    |
| Scaffold-DbContext       | Generates a DbContext and entity type classes for a specified database. This is called reverse engineering. |
| Get-DbContext            | Gets information about a DbContext type.                     |
| Drop-Database            | Drops the database.                                          |

For examples:

```
## To generate database migration code files automatically
Add-Migration AddGenericOption_WebAPILog
## To execute database migration code files and apply modifications to current connected database file
Update-Database
```

## Layouts

### WebUI

This layer is a web application. This layer depends on both the Application and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. Therefore only *Startup.cs* should reference Infrastructure.

### Infrastructure

This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

### Application

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

### Domain

This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

## Overview

- Web UI/API
- Infrastructure
  - Common
  - Extension
  - Identity
  - Persistence
    - DBContext
    - Migrations
  - Services
  - ConfigureServices.cs
- Applications
  - Common
    - Behavious
      - IPipelineBehavior implementations of MediatR
    - Exceptions
    - Mappings
    - Models (DTO)
    - Attribute
  - Configurations
  - Interfaces
    - Repositories
  - Extensions
  - Exceptions
  - Features
    - {BusinessDomainEntity}
      - Commands
        - [Create/Update/Delete/Import]{Entity}
          - Command.cs
            - Inject Reader
            - Inject Writer
          - CommandValidator.cs
      - Queries
        - Get{Entity}WithPagination
          - Query.cs
            - Inject Reader
          - QueryValidator.cs
      - EventHandlers
  - ConfigureServices.cs
- Domain
  - Common
    - BaseAuditableEntity.cs
    - BaseEntity.cs
    - BaseEvent.cs
    - ValueObject.cs
  - Entities
  - Enums
  - Events
  - Exceptions
  - ValueObjects

## TODO List

- [ ] Tethys - TemplateService
- [ ] Oceanus - CommonLibrary
- [ ] Uranus - Gateway
- [ ] Prometheus - LogService
- [ ] Themis - IdentityServer
- [ ] Aphrodite - WebUI
