# Progress Details: 03.02-dependent-net10

## Changes
- Retargeted `ClearUpdates`, `StatsRotation`, `Bot Watcher`, and `Updater` to .NET 10.
- Retargeted `ClearUpdates` to `net10.0-windows` because it reads the Windows registry.
- Removed legacy framework-included System.* PackageReferences and explicit assembly references from these projects.
- Updated retained EntityFramework references to 6.5.2 and Newtonsoft.Json to 13.0.4.
- Replaced obsolete `Assembly.CodeBase` path resolution with `AppContext.BaseDirectory` in Bot Watcher and Updater.
- Removed obsolete `ServicePointManager.SecurityProtocol` assignment from Updater.
- Added a Windows platform annotation to the ClearUpdates entry point for registry access.

## Validation
- ClearUpdates `dotnet build`: passed with 0 warnings on `net10.0-windows`.
- StatsRotation `dotnet build`: passed with 0 warnings on `net10.0`.
- Bot Watcher `dotnet build`: passed with 0 warnings on `net10.0`.
- Updater `dotnet build`: passed with 0 warnings on `net10.0`.
- Outputs were generated under the expected net10.0/net10.0-windows directories.

## Deferred Work
- ClearUpdates and other applications retain legacy Telegram.Bot APIs; package/API modernization remains deferred unless downstream migration builds require it.
- Existing runtime behavior involving Windows registry and process launching still requires Windows runtime validation.
