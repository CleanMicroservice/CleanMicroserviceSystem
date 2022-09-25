# CleanMicroserviceSystem

<p align="center">
   <img src="https://raw.github.com/FinancialMarketSimulator/CleanMicroserviceSystem/master/doc/Logo.png" align="center"/>
   <h2 align="center">Clean Microservice System</h2>
   <p align="center">A Microservice System template repository based on Clean Architecture and ASP.NET Core Blazor Web Assembly.</p>
</p>

## Status

<p align="center">
   <a href="https://github.com/FinancialMarketSimulator/CleanMicroserviceSystem/actions/workflows/dotnet-core.yml">
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

## Layouts

### Domain

This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

### Application

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

### Infrastructure

This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

### WebUI

This layer is a single page application based on Angular 13 and ASP.NET Core 6. This layer depends on both the Application and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. Therefore only *Startup.cs* should reference Infrastructure.

## Book list

- [Microservices [EN-US]](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/)
- [microservices [ZH-CN]](https://learn.microsoft.com/zh-cn/dotnet/architecture/microservices/)

## Overview

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
- Web UI/API

## TODO List

- [ ] Oceanus - TemplateService
- [ ] Uranus - Gateway
- [ ] Prometheus - LogService
- [ ] Themis - IdentityServer
