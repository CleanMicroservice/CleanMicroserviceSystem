# CleanMicroserviceSystem

<p align="center">
   <img src="https://raw.github.com/CleanMicroservice/CleanMicroserviceSystem/master/doc/Logo.png" align="center"/>
   <h2 align="center">Clean Microservice System</h2>
   <p align="center">A Microservice System template repository based on Clean Architecture and ASP.NET Core Blazor Web Assembly.</p>
</p>

## Status

<p align="center">
   <a href="https://github.com/CleanMicroservice/CleanMicroserviceSystem/actions/workflows/dotnet.yml">
      <img border="0" src="https://github.com/CleanMicroservice/CleanMicroserviceSystem/workflows/.Net%20Build/badge.svg" />
   </a>
   <a href="https://github.com/CleanMicroservice/CleanMicroserviceSystem/blob/master/LICENSE">
      <img border="0" src="https://img.shields.io/github/license/CleanMicroservice/CleanMicroserviceSystem" />
   </a>
   <a href="https://github.com/CleanMicroservice/CleanMicroserviceSystem/search?l=c%23">
      <img border="0" src="https://img.shields.io/github/languages/top/CleanMicroservice/CleanMicroserviceSystem" />
   </a>
   <a href="https://github.com/CleanMicroservice/CleanMicroserviceSystem">
      <img border="0" src="https://img.shields.io/github/directory-file-count/CleanMicroservice/CleanMicroserviceSystem" />
   </a>
   <a href="https://github.com/CleanMicroservice/CleanMicroserviceSystem/archive/refs/heads/master.zip">
      <img border="0" src="https://img.shields.io/github/repo-size/CleanMicroservice/CleanMicroserviceSystem" />
   </a>
   <a href="https://github.com/CleanMicroservice/CleanMicroserviceSystem/issues?q=is%3Aopen+is%3Aissue">
      <img border="0" src="https://img.shields.io/github/issues/CleanMicroservice/CleanMicroserviceSystem" />
   </a>
   <a href="https://github.com/CleanMicroservice/CleanMicroserviceSystem/network/members">
      <img border="0" src="https://img.shields.io/github/forks/CleanMicroservice/CleanMicroserviceSystem" />
   </a>
   <a href="https://github.com/CleanMicroservice/CleanMicroserviceSystem/stargazers">
      <img border="0" src="https://img.shields.io/github/stars/CleanMicroservice/CleanMicroserviceSystem" />
   </a>
   <a href="https://github.com/CleanMicroservice/CleanMicroserviceSystem/watchers">
      <img border="0" src="https://img.shields.io/github/watchers/CleanMicroservice/CleanMicroserviceSystem" />
   </a>
   <a href="https://github.com/CleanMicroservice/CleanMicroserviceSystem/releases">
      <img border="0" src="https://img.shields.io/github/v/release/CleanMicroservice/CleanMicroserviceSystem?include_prereleases" />
   </a>
   <a href="https://github.com/CleanMicroservice/CleanMicroserviceSystem/releases">
      <img border="0" src="https://img.shields.io/github/release-date-pre/CleanMicroservice/CleanMicroserviceSystem" />
   </a>
   <a href="https://github.com/CleanMicroservice/CleanMicroserviceSystem/archive/refs/heads/master.zip">
      <img border="0" src="https://img.shields.io/github/downloads/CleanMicroservice/CleanMicroserviceSystem/total" />
   </a>
   <a href="https://github.com/CleanMicroservice/CleanMicroserviceSystem/tags">
      <img border="0" src="https://img.shields.io/github/v/tag/CleanMicroservice/CleanMicroserviceSystem" />
   </a>
   <a href="https://github.com/CleanMicroservice/CleanMicroserviceSystem/releases">
      <img border="0" src="https://img.shields.io/github/commits-since/CleanMicroservice/CleanMicroserviceSystem/latest/master?include_prereleases" />
   </a>
   <a href="https://github.com/CleanMicroservice/CleanMicroserviceSystem/commits/master">
      <img border="0" src="https://img.shields.io/github/last-commit/CleanMicroservice/CleanMicroserviceSystem/master" />
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

| Command                  | Description                                                                                                 |
| ------------------------ | ----------------------------------------------------------------------------------------------------------- |
| Get-Help entityframework | Displays information about entity framework commands.                                                       |
| Add-Migration            | Creates a migration by adding a migration snapshot.                                                         |
| Remove-Migration         | Removes the last migration snapshot.                                                                        |
| Update-Database          | Updates the database schema based on the last migration snapshot.                                           |
| Script-Migration         | Generates a SQL script using all the migration snapshots.                                                   |
| Scaffold-DbContext       | Generates a DbContext and entity type classes for a specified database. This is called reverse engineering. |
| Get-DbContext            | Gets information about a DbContext type.                                                                    |
| Drop-Database            | Drops the database.                                                                                         |

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
  - [ ] 泰西斯，Oceanus的妻子。是[希腊神话](https://baike.baidu.com/item/希腊神话/63444?fromModule=lemma_inlink)中的海之女神。她是希腊第一位、最初的海洋女神，因此在[希腊语](https://baike.baidu.com/item/希腊语/675775?fromModule=lemma_inlink)中，泰西斯也是“祖母”的意思，在早期神话中，她是一位[创世女神](https://baike.baidu.com/item/创世女神/12853246?fromModule=lemma_inlink)。她与大洋神俄刻阿诺斯生下的众多子女里，每一个孩子都代表着小溪、河流或者大海。
- [ ] Oceanus - CommonLibrary
  - [ ] 俄刻阿诺斯，Tethys 的丈夫。[十二提坦](https://baike.baidu.com/item/十二提坦/6074612?fromModule=lemma_inlink)神，水神大洋神，他是那条环绕着宇宙转动的河流腰带，故而他的结尾也是开端：这条宇宙之河自我组成一个圆圈在转动。
- [ ] Uranus - Gateway
  - [ ] [乌拉诺斯](https://baike.baidu.com/item/乌拉诺斯/3398955?fromModule=lemma_inlink)，第一代神王、天空之神。从大地之神[盖亚](https://baike.baidu.com/item/盖亚/33005?fromModule=lemma_inlink)的指端诞生，最初作为宇宙统治者的第一代众神之王、即天空的神格化。象征希望与未来，并代表天空。十二泰坦巨神。
- [ ] Prometheus - LogService
  - [ ] 普罗米修斯曾与智慧女神[雅典娜](https://baike.baidu.com/item/雅典娜/26005?fromModule=lemma_inlink)共同创造了人类，普罗米修斯负责用泥土雕塑出人的形状，[雅典娜](https://baike.baidu.com/item/雅典娜/26005?fromModule=lemma_inlink)则为泥人灌注灵魂，并教会了人类很多知识。普罗米修斯还反抗宙斯，将火种带到人间。
- [ ] Themis - IdentityServer
  - [ ] 忒弥斯（Θεμις / Themis，“法律”）是[法律](https://baike.baidu.com/item/法律/84813?fromModule=lemma_inlink)和正义的象征。忒弥斯是宙斯最尊重最信任的妻子。是秩序的创造者、守护者。
- [ ] Aphrodite - WebUI
  - [ ] 阿佛洛狄忒，是[古希腊神话](https://baike.baidu.com/item/古希腊神话/1962436?fromModule=lemma_inlink)中爱情与美丽的女神，同时也是性欲女神，[奥林匹斯十二主神](https://baike.baidu.com/item/奥林匹斯十二主神/34825?fromModule=lemma_inlink)之一。因其诞生于海洋，所以有时还被奉为航海的庇护神。阿佛洛狄忒生于海中浪花，拥有白瓷般的肌肤、金发碧眼和古希腊女性完美的身材和相貌，象征女性的美丽，被认为是女性身体美的最高象征。
