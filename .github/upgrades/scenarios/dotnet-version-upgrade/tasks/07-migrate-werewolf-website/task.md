# 07-migrate-werewolf-website: Incrementally migrate Werewolf Website to ASP.NET Core

Migrate the Werewolf Website web surface to the scaffolded ASP.NET Core project using System.Web Adapters during the transition. Replace Global.asax initialization and unsupported bundling/minification, migrate configuration and web assets, update package references, and resolve compatibility issues in dependency order while preserving the legacy site.

**Done when**: Werewolf Website functionality runs on .NET 10, routes/views/assets and initialization are validated, package and API issues are resolved or tracked, tests/builds pass, and the legacy project remains available pending production cutover.
