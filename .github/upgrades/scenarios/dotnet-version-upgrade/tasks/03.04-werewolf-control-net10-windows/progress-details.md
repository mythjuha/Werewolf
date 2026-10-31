# Progress Details: 03.04-werewolf-control-net10-windows

## Changes
- Added `Microsoft.Extensions.Caching.Memory` version `# Progress Details: 03.04-werewolf-control-net10-windows

## Changes
- Added `Microsoft.Extensions.Caching.Memory` version `10.0.0` to `WerewolfControl.csproj`.
- Replaced `System.Runtime.Caching.MemoryCache` with `Microsoft.Extensions.Caching.Memory.IMemoryCache` and `MemoryCache` in `Helpers/UpdateHelper.cs`.
- Replaced the legacy cache indexer and `CacheItemPolicy` with `TryGetValue` and `MemoryCacheEntryOptions.AbsoluteExpirationRelativeToNow` set to one hour.
- Added `ScottPlot.WinForms` version `4.1.73` to `WerewolfControl.csproj`.
- Replaced `System.Windows.Forms.DataVisualization.Charting` usage in `Helpers/Charting.cs` with headless ScottPlot `Plot` generation and PNG export.
- Replaced the developer monthly chart in `Commands/DevCommands.cs` with ScottPlot and JPEG export while preserving the Telegram upload flow.
- Removed all DataVisualization charting imports and API usages from the Werewolf Control source.
- Resolved namespace collisions introduced by ScottPlot aliases for the existing application `Settings`, `System.Version`, and `System.Drawing.Color` types.
- Persisted the selected ScottPlot charting preference in `scenario-instructions.md`.

## Validation
- `UpdateHelper.cs` has no editor/compiler errors.
- Targeted project build restores the new cache package successfully and no longer reports `System.Runtime.Caching` errors.
- Full Werewolf Control compilation remains blocked by unrelated missing `System.Windows.Forms.DataVisualization` types in `Commands/DevCommands.cs` and `Helpers/Charting.cs`.
- Existing NU1701 warnings remain for legacy Team Foundation/Visual Studio Services, TPL Dataflow, Web Xdt, and TLSharp packages.
- `WerewolfControl.csproj` builds successfully for `net10.0-windows` with `run_build`.
- The build output was verified under `Werewolf for Telegram/Werewolf Control/bin/Debug/net10.0-windows/`.
- Source search found no remaining `DataVisualization`, `ChartImageFormat`, `SeriesChartType`, `ChartArea`, or `new Chart` usages in the project source.

## Deferred Work
- Charting dependency migration remains required before Werewolf Control can build successfully on net10.0-windows.
- Legacy package compatibility and security remediation remain deferred according to the upgrade policy.
- Existing NU1701 warnings from legacy packages remain part of the broader Werewolf Control migration and are not caused by the ScottPlot replacement.
- Runtime Telegram chart delivery was not exercised because it requires the bot service and database environment; the generated file paths and upload flow remain intact.
