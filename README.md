# CleanMicroserviceSystem

A Microservice System template repository based on Clean Architecture and ASP.NET Core Blazor Web Assembly

## Layouts

### Domain

This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

### Application

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

### Infrastructure

This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

### WebUI

This layer is a single page application based on Angular 13 and ASP.NET Core 6. This layer depends on both the Application and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. Therefore only *Startup.cs* should reference Infrastructure.

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
