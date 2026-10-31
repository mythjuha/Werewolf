# 06-scaffold-werewolf-website: Scaffold the ASP.NET Core Werewolf Website proxy

Create a new ASP.NET Core `net10.0` project alongside the existing `Werewolf Website` Framework application and configure YARP to proxy requests to the legacy site. Establish a buildable Core host without deleting or disabling the existing deployment.

## Research findings

- The Core host already exists at `Werewolf for Telegram/WerewolfWebsite.Core` and targets `net10.0`.
- It references the upgraded `Database` project and `Microsoft.EntityFrameworkCore.SqlServer`; `WWCoreContext` exposes the existing `AspNetUser`, role, group, and statistics tables.
- YARP and System.Web Adapters are registered in `Program.cs`, `ProxyTo` is configured, and the Core project builds successfully.
- The legacy website still owns Account, Manage, AdminApi, and Home EF JSON actions. The next implementation slice will port these endpoints into the Core host while leaving the Framework website intact.
- Legacy Identity uses the existing `AspNetUsers`/`AspNetRoles` schema. The Core implementation will preserve that schema, use cookie authentication, and use EF Core queries against `WWCoreContext` rather than introducing a second Identity database context.

**Done when**: The new Werewolf Website Core project is in the solution, restores and builds on .NET 10, returns a validated proxied response, and the old website remains deployable.
