# 03.02-dependent-net10: Migrate low-risk dependent projects to .NET 10

## Objective
Migrate ClearUpdates, StatsRotation, Bot Watcher, and Updater after foundation projects build on .NET 10.

## Scope
- Update target frameworks and compatible package references.
- Resolve simple source/API and binding issues.
- Restore and build in dependency order.
- Track complex or unsupported package/API work for follow-up.

## Research Findings

- `ClearUpdates` targets `net472`, references the newly migrated `Database` project, and contains a large set of old `NETStandard.Library`/System.* package references generated during legacy conversion. It uses EntityFramework 6.2.0-beta1, Newtonsoft.Json 11.0.2, and Telegram.Bot 14.11.0. Assessment recommends EntityFramework 6.5.2 and Newtonsoft.Json 13.0.4.
- `StatsRotation` targets `net472`, references `Database`, and uses EntityFramework 6.1.3. Its explicit framework references are no longer appropriate for net10.0 and should be removed after source inspection/build.
- `Bot Watcher` targets `net48`, is a simple executable with no NuGet packages, and copies `processes.txt` to output. Its project file is `Bot Watcher/Bot Watcher.csproj` (the space is part of the filename).
- `Updater` targets `net48`, has no NuGet packages, and uses explicit framework references. Preserve its `AssemblyName` value `Update` and configurations while retargeting.
- All four projects use standard per-project package management; dependency inspection found no CPM.
- The migrated Database project now targets net10.0 and uses EntityFramework 6.5.2, so dependent EF references should be aligned to 6.5.2 where retained.

## Execution Decisions

- Replace the single target framework in all four projects with `net10.0`.
- Remove framework-included System.* PackageReferences and explicit framework assembly references when the code does not require an external package; retain only genuine third-party packages.
- Upgrade retained EntityFramework references to 6.5.2 and Newtonsoft.Json to 13.0.4 using the supported stable versions identified by assessment.
- Do not modernize Telegram.Bot in this subtask; its legacy API compatibility is a deferred complex change unless the net10 build cannot compile.
- Preserve executable output behavior, custom configurations, `AssemblyName`, and copied content.

## Validation

1. Restore/build ClearUpdates after Database is on net10.0.
2. Restore/build StatsRotation.
3. Restore/build Bot Watcher and Updater.
4. Fix all warnings in touched projects; do not suppress warnings.
5. Confirm each output is under `bin/.../net10.0` and record deferred Telegram/API/binding issues in `progress-details.md`.
