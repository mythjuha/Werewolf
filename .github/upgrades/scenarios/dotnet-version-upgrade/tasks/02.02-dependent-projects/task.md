# 02.02-dependent-projects: Convert dependent non-web projects to SDK-style

## Objective
Convert non-web projects that depend on the foundation libraries after their dependencies are stable.

## Scope
ClearUpdates and StatsRotation depend on Database; Werewolf Node and Werewolf Control depend on Database, Shared, and TcpFramework. Preserve all project references and legacy package versions during conversion.

## Research Findings

- `ClearUpdates` and `StatsRotation` depend on `Database` and were converted after the foundation projects.
- `Werewolf Node` and `Werewolf Control` depend on `Database`, `Shared`, and `TcpFramework` and contain large legacy `packages.config` inventories.
- `Werewolf Node` and `Werewolf Control` both contained a malformed `Telegram.Bot.19.0.0-preview` package reference generated from the original prerelease `Telegram.Bot` 19.0.0-preview.1 entry; the malformed generated reference was removed while the original package reference was retained.
- `Werewolf Node` and `Werewolf Control` use wildcard `AssemblyVersion("4.3.*")`; SDK deterministic compilation was disabled to preserve the existing versioning behavior.
- Existing package versions were otherwise preserved. Package compatibility upgrades remain deferred to the framework migration task.
- Conversion tool ordering used: `ClearUpdates`, `StatsRotation`, `Werewolf Node`, then `Werewolf Control`.

## Steps
1. Confirm the foundation conversion subtask completed successfully.
2. Convert ClearUpdates, StatsRotation, Werewolf Node, and Werewolf Control sequentially with convert_project_to_sdk_style.
3. Build each converted project and its converted dependencies using Visual Studio MSBuild.
4. Verify packages.config migration, assembly references, resources, and project references.

## Done when
All four dependent projects are SDK-style, retain their original target frameworks and package versions, and build without warnings or errors.
