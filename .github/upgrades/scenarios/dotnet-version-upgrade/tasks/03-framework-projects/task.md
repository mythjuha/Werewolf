# 03-framework-projects: Upgrade non-web projects to .NET 10

Upgrade all non-web project target frameworks toward `net10.0` in dependency order, using multi-targeting only where old Framework consumers must remain temporarily and in-place targeting where the dependency graph allows it. Resolve package updates, deprecated/vulnerable references, binding configuration, mechanical API changes, and source incompatibilities as each dependency group is migrated. Complex incompatible packages and API changes may be staged according to the confirmed deferred-resolution policy.

This task covers `Database`, `TcpFramework`, `Shared`, `ClearUpdates`, `Bot Watcher`, `StatsRotation`, `Updater`, `Werewolf Node`, `Werewolf Control`, and `Languages`. `Werewolf Control` is a Windows Forms application and therefore requires the platform-specific `net10.0-windows` target rather than portable `net10.0`. `BuildAutomation` was discovered to be an ASP.NET Framework WAP with `Global.asax`/`System.Web` and is excluded from this non-web task; it requires a side-by-side web migration task.

## Research Findings

- All converted non-web `.csproj` files currently retain their original `net472` or `net48` targets; no `Directory.Build.props` or `Directory.Build.targets` overrides them.
- Foundation libraries: `Database`, `TcpFramework`, and `Shared` have mostly framework-included System.* package references. `Database` recommends EntityFramework 6.5.2; `Shared` has a legacy cryptography API finding.
- Dependent applications: `ClearUpdates` has 53 assessment findings including two vulnerable packages, EntityFramework/Newtonsoft.Json updates, and many framework-included package references. `StatsRotation` has a source API issue and binding redirect configuration. `Werewolf Node` has legacy cryptography, 7 source and 3 behavioral API findings, binding redirect conflicts, and a large legacy package inventory.
- `Werewolf Control` is a large WinForms project with target recommendation `net10.0-windows`, 174 source API findings, 5 binary incompatibilities, 27 GDI+ findings, 125 WinForms findings, incompatible packages (`BigMath`, `BotanIO.Net`, Team Foundation clients, `TLSharp`), and vulnerable `DotNetZip`/`log4net` packages. It should be migrated last among non-web projects.
- `Bot Watcher` and `Updater` have source/behavioral API findings but no package inventory; they can be handled after their project references are stable.
- `Languages.shproj` is not SDK-style and requires project-specific handling rather than a `.csproj` target edit.
- `BuildAutomation` is excluded because it is a web project; retain it on Framework until a scaffold/migrate task is added.

## Execution Constraints

- Upgrade in dependency order: `Database`, `TcpFramework`, `Shared`; then `ClearUpdates` and `StatsRotation`; then `Bot Watcher`, `Updater`, and `Werewolf Node`; then `Werewolf Control`; handle `Languages` separately.
- Remove framework-included System.* package references only when the target build confirms they are unnecessary; do not remove package references blindly.
- Use `net10.0-windows` for Werewolf Control and preserve Windows Forms support.
- Apply mechanical API replacements inline; mark complex API/package work with the standardized `// STUB:` markers and track follow-up work.
- Build each dependency group before moving upward. There are no test projects in the solution.

**Done when**: All non-web projects target or appropriately multi-target .NET 10, package restore and builds succeed in dependency order, simple compatibility fixes are complete, deferred issues are explicitly tracked, and each dependency group passes its available tests.
