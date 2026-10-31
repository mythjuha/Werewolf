# .NET Version Upgrade Plan

## Overview

**Target**: Upgrade all projects in `WerewolfForTelegram.sln` from .NET Framework 4.7.2/4.8 to .NET 10 (`net10.0`), including SDK-style conversion, package and API compatibility work, and migration of the ASP.NET Framework web applications.
**Scope**: 12-project solution with legacy project formats, two System.Web web applications, incompatible/deprecated/vulnerable packages, binding redirects, and source/behavioral API compatibility findings.

### Selected Strategy
**Bottom-Up (Dependency-First)** — Upgrade from leaf nodes to root applications, tier by tier.
**Rationale**: 12 projects span a multi-level dependency graph and cross the .NET Framework-to-modern-.NET boundary, so dependency-first validation is required.

Dependency order from the solution graph:

```text
Foundation: Database, TcpFramework, Languages
	↓
Shared libraries: Shared
	↓
Applications and services: DonationSite, ClearUpdates, Bot Watcher, BuildAutomation,
StatsRotation, Updater, Werewolf Node, Werewolf Website, Werewolf Control
```

The exact project references and tier breakdown will be confirmed during task execution. Side-by-side web migration is used for `DonationSite` and `Werewolf Website`; their legacy projects remain in place while new ASP.NET Core projects are scaffolded and migrated.

## Tasks

### 01-prerequisites: Verify the .NET 10 upgrade toolchain

Verify that the .NET 10 SDK, required Visual Studio/MSBuild components, and any `global.json` constraints support the target framework. Establish the baseline build/test status and record the available test projects before changing project files.

**Done when**: The .NET 10 SDK and repository SDK configuration are compatible, baseline validation is recorded, and the solution's upgrade prerequisites are satisfied or documented as blockers.

---

### 02-sdk-conversion: Convert non-web legacy projects to SDK-style

Convert the non-web legacy `.csproj` projects identified by the assessment to SDK-style while keeping their current .NET Framework target. Migrate `packages.config` references to `PackageReference` where present and preserve project references, generated files, resources, build actions, and application behavior. The side-by-side web projects are excluded because their existing Framework projects remain live and the scaffolded Core projects are already SDK-style.

Affected non-web projects include `Bot Watcher`, `BuildAutomation`, `ClearUpdates`, `Database`, `Shared`, `StatsRotation`, `TcpFramework`, `Updater`, `Werewolf Control`, and `Werewolf Node`; `Languages.shproj` requires separate target-framework handling.

**Done when**: Every eligible non-web project is SDK-style, restores successfully on its existing Framework target, preserves required references/resources, and builds without warnings or errors.

---

### 03-framework-projects: Upgrade non-web projects to .NET 10

Upgrade all non-web project target frameworks toward `net10.0` in dependency order, using multi-targeting only where old Framework consumers must remain temporarily and in-place targeting where the dependency graph allows it. Resolve package updates, deprecated/vulnerable references, binding configuration, mechanical API changes, and source incompatibilities as each dependency group is migrated. Complex incompatible packages and API changes may be staged according to the confirmed deferred-resolution policy.

This task covers `Database`, `TcpFramework`, `Shared`, `ClearUpdates`, `Bot Watcher`, `BuildAutomation`, `StatsRotation`, `Updater`, `Werewolf Node`, `Werewolf Control`, and `Languages`, with per-project breakdown and validation required because the assessment reports incompatibilities across several layers.

**Done when**: All non-web projects target or appropriately multi-target .NET 10, package restore and builds succeed in dependency order, simple compatibility fixes are complete, deferred issues are explicitly tracked, and each dependency group passes its available tests.

---

### 04-scaffold-donationsite: Scaffold the ASP.NET Core DonationSite proxy

Create a new ASP.NET Core `net10.0` project alongside the existing `DonationSite` Framework application and configure the YARP reverse proxy for side-by-side operation. Preserve the existing application as the fallback/live implementation while the new project establishes a buildable Core host and a validated proxy route.

**Done when**: The new DonationSite Core project is in the solution, restores and builds on .NET 10, routes a known request through YARP to the existing application, and the old project remains deployable.

---

### 05-migrate-donationsite: Incrementally migrate DonationSite to ASP.NET Core

Migrate the DonationSite web surface from System.Web to the scaffolded ASP.NET Core project while using System.Web Adapters where needed. Address Global.asax initialization, bundling/minification replacement, configuration, controllers/views, middleware, package compatibility, and the shared-library references used by the site. Keep the Framework application live until the Core implementation is validated.

**Done when**: DonationSite functionality is migrated and validated on .NET 10, adapter usage is limited to required compatibility seams, routes and static assets work, tests/builds pass, and the legacy project remains available for rollback and post-production removal.

---

### 06-scaffold-werewolf-website: Scaffold the ASP.NET Core Werewolf Website proxy

Create a new ASP.NET Core `net10.0` project alongside the existing `Werewolf Website` Framework application and configure YARP to proxy requests to the legacy site. Establish a buildable Core host without deleting or disabling the existing deployment.

**Done when**: The new Werewolf Website Core project is in the solution, restores and builds on .NET 10, returns a validated proxied response, and the old website remains deployable.

---

### 07-migrate-werewolf-website: Incrementally migrate Werewolf Website to ASP.NET Core

Migrate the Werewolf Website web surface to the scaffolded ASP.NET Core project using System.Web Adapters during the transition. Replace Global.asax initialization and unsupported bundling/minification, migrate configuration and web assets, update package references, and resolve compatibility issues in dependency order while preserving the legacy site.

**Done when**: Werewolf Website functionality runs on .NET 10, routes/views/assets and initialization are validated, package and API issues are resolved or tracked, tests/builds pass, and the legacy project remains available pending production cutover.

---

### 08-solution-validation: Validate the complete .NET 10 solution

Run restore, a warning-free full solution build, and all discovered tests after both web migrations. Review deferred package/API items, document binding redirects and any remaining compatibility work, confirm project references and deployment configuration, and record central package management as a post-migration recommendation once all projects are stable on one modern TFM.

**Done when**: The full solution builds without errors or warnings, all available tests pass, no unresolved dependency conflicts remain, deferred items are explicitly documented, and post-upgrade web-project removal/cutover and CPM adoption are clearly identified for the user.
