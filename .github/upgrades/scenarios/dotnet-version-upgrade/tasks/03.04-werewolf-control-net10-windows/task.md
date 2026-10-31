# 03.04-werewolf-control-net10-windows: Migrate Werewolf Control to net10.0-windows

## Objective
Migrate the WinForms Werewolf Control application to net10.0-windows.

## Scope
- Preserve UseWindowsForms and Windows-specific behavior.
- Update or remove incompatible/vulnerable packages where safe.
- Resolve simple WinForms, System.Drawing, and API issues.
- Defer complex package/API replacements with explicit tracking.
- Restore and build the Windows-targeted project.

## Research Findings

- `WerewolfControl.csproj` targets `net10.0-windows`, enables WinForms, and references Database/Shared/TcpFramework on net10.
- `Helpers/UpdateHelper.cs` uses `Microsoft.Extensions.Caching.Memory` with one-hour absolute expiration after the user's cache replacement decision.
- The project uses standard per-project package references; no CPM is configured.
- `Helpers/Charting.cs` and `Commands/DevCommands.cs` depended on unavailable `System.Windows.Forms.DataVisualization.Charting` types.
- NuGet resolution returned stable `ScottPlot.WinForms` version `4.1.73` for `net10.0-windows`. The 4.x API supports headless PNG/JPEG export through `ScottPlot.Plot`, which avoids embedding a Forms control in command code.

## Execution Decisions

- Add `ScottPlot.WinForms` version `4.1.73` as the charting dependency.
- Replace DataVisualization chart construction with ScottPlot `Plot` instances and `AddScatter` series, preserving output files and Telegram upload behavior.
- Keep chart generation headless because both callers generate files for Telegram rather than displaying a WinForms control.
- Preserve the existing SQL query, team colors, chart dimensions, and monthly aggregation semantics as closely as supported by ScottPlot.
- Keep the existing Microsoft.Extensions.Caching.Memory replacement and one-hour expiration semantics.

## Affected Files

- `Werewolf for Telegram/Werewolf Control/WerewolfControl.csproj`: add ScottPlot.WinForms 4.1.73.
- `Werewolf for Telegram/Werewolf Control/Helpers/Charting.cs`: replace DataVisualization chart creation and PNG export with ScottPlot.
- `Werewolf for Telegram/Werewolf Control/Commands/DevCommands.cs`: replace the developer monthly chart path and JPEG export with ScottPlot.

## Validation

1. Restore and build `WerewolfControl.csproj` on `net10.0-windows`.
2. Confirm no source references remain to `System.Windows.Forms.DataVisualization.Charting`.
3. Confirm the output assembly is generated under the net10.0-windows output folder.
4. Review remaining warnings and document legacy-package warnings or unrelated deferred issues in `progress-details.md`.
