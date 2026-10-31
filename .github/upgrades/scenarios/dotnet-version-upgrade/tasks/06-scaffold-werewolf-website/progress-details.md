# Progress Details

## Changes
- Added `WerewolfWebsite.Core`, an ASP.NET Core MVC project targeting `net10.0`, beside the legacy ASP.NET Framework Website.
- Added YARP 2.3.0 and Microsoft.AspNetCore.SystemWebAdapters.CoreServices 2.3.0.
- Configured `WebApplication.CreateBuilder(args)` host startup, MVC, static-file middleware, System.Web adapters, YARP forwarding, and a `/health` smoke-test endpoint.
- Configured the legacy Website fallback as `https://localhost:44307` in appsettings and launch settings.
- Added the Core project to `WerewolfForTelegram.sln` and linked the legacy project with `_MigrateToProjectGuid`.
- Preserved the legacy `Werewolf Website` project unchanged as the rollback/live deployment.
- Added the migrated Database project reference and EF Core SQL Server registration.
- Copied the legacy Content, Scripts, plugins, images, fonts, and favicon assets into `wwwroot`.
- Migrated the anonymous Home controller and About, Contact, Achievements, and Home views.
- Replaced MVC bundling and `System.Web` view helpers with ASP.NET Core static links, Tag Helpers, `ViewData`, and `RenderSectionAsync`.
- Migrated the public Stats page/controller shell and converted GlobalStats, DailyCounts, group stats, player stats, player kill/death lists, and player achievements to `WWCoreContext` async EF Core queries.
- Added a configurable `ConnectionStrings:BotConnectionString` entry without committing a secret.

## Validation
- `dotnet build WerewolfWebsite.Core/WerewolfWebsite.Core.csproj` passed with zero warnings and zero errors after the expanded Stats migration.
- Started the Core host on `http://localhost:5224` and verified `/health`, `/Home/Index`, `/Home/Achievements`, and `/Stats/Index` returned HTTP 200.
- Verified `/Stats/GlobalStats` and `/Stats/DailyCounts` return HTTP 503 when the database connection is not configured, instead of an unhandled HTTP 500.
- Verified `/Stats/GroupStats`, `/Stats/PlayerStats`, `/Stats/PlayerKills`, `/Stats/PlayerKilledBy`, `/Stats/PlayerDeaths`, `/Stats/PlayerAchievements`, and `/Stats/PlayerLockedAchievements` return HTTP 503 without database configuration.

## Runtime notes
- YARP fallback requires the legacy Framework site to be running at the configured URL.
- A valid database connection must be supplied through user secrets, environment variables, or deployment configuration before Stats data endpoints can return data.
- Account, Manage, Admin API, and EF-dependent Home JSON actions remain proxied and are the next migration scope.

## Continued migration
- Added cookie authentication and Developer-role authorization to the Core pipeline.
- Ported Account login, registration, logout, access-denied handling, password hashing, and role claims against the existing `AspNetUsers`/`AspNetRoles` schema.
- Ported Manage account overview and password change flows.
- Ported Home `GetPublicGroups` as an async EF Core query and preserved `GetBotStatus`.
- Added the Admin API route surface with Developer authorization and Core JSON responses; the legacy TCP status provider remains a documented follow-up seam.
- Added minimal Account and Manage Razor views.

## Continued validation
- `WerewolfWebsite.Core.csproj` builds successfully on `net10.0` with no project-level warnings.
- The Core host ran on `http://localhost:5189`; `/health` returned HTTP 200.
- The legacy Framework website remains present and deployable.
- The full solution reached the Core project but is not warning-free because unrelated legacy `Werewolf Control` diagnostics remain.
