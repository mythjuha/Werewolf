# 02-sdk-conversion: Convert non-web legacy projects to SDK-style

Convert the non-web legacy `.csproj` projects identified by the assessment to SDK-style while keeping their current .NET Framework target. Migrate `packages.config` references to `PackageReference` where present and preserve project references, generated files, resources, build actions, and application behavior. The side-by-side web projects are excluded because their existing Framework projects remain live and the scaffolded Core projects are already SDK-style.

Affected non-web projects include `Bot Watcher`, `BuildAutomation`, `ClearUpdates`, `Database`, `Shared`, `StatsRotation`, `TcpFramework`, `Updater`, `Werewolf Control`, and `Werewolf Node`; `Languages.shproj` requires separate target-framework handling.

## Scope Inventory

- **Projects affected**: 10 legacy non-web `.csproj` projects. `DonationSite` and `Werewolf Website` remain legacy for side-by-side migration; `Languages.shproj` is not converted by the `.csproj` conversion tool.
- **Distinct concerns**: SDK project conversion, `packages.config` to `PackageReference`, preservation of assembly/resource metadata, project-reference compatibility, and Framework-target build validation.
- **Dependency order**: Convert leaf projects first: `Database`, `TcpFramework`, `Shared`, `Bot Watcher`, `BuildAutomation`, and `Updater`; then `ClearUpdates` and `StatsRotation` (both depend on `Database`); then `Werewolf Node` and `Werewolf Control` (both depend on `Database`, `Shared`, and `TcpFramework`). The topological tool returned this order with web projects interleaved by solution structure.
- **Assessment signals**: Every affected project is non-SDK-style and requires project-format conversion. `ClearUpdates`, `BuildAutomation`, `Werewolf Node`, and `Werewolf Control` also have package compatibility/update/deprecation/security findings that must not be resolved as part of format-only conversion unless conversion itself exposes a build issue.
- **Package inventory**: `Database`, `TcpFramework`, `Shared`, `ClearUpdates`, `BuildAutomation`, `StatsRotation`, `Werewolf Node`, and `Werewolf Control` use `packages.config`; conversion must preserve package versions and migrate references without introducing target-framework or package-version changes.
- **Special compatibility**: `Werewolf Control` contains `System.Web`, Windows Forms, drawing, and caching assembly references; preserve these during conversion and use Visual Studio MSBuild for validation.
- **Conversion constraints**: Use `convert_project_to_sdk_style` one project at a time, never manually rewrite project XML, do not change target frameworks, and build each converted project before moving to dependent projects.

**Done when**: Every eligible non-web project is SDK-style, restores successfully on its existing Framework target, preserves required references/resources, and builds without warnings or errors.
