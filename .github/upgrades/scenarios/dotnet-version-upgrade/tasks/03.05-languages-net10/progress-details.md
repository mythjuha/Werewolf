# Progress Details

## Research

- Inspected `Languages/Languages.shproj` and the Node language-loading and post-build deployment behavior.
- Confirmed the shared project contains resources rather than a separately compiled assembly and does not require a .NET 10 target framework.

## Related Host Startup Work

At the user's request, the following .NET 10 executable projects were updated to use `Host.CreateApplicationBuilder` and Generic Host lifecycle methods:

- `Werewolf Control`
- `Werewolf Node`
- `Bot Watcher`
- `Updater`
- `ClearUpdates`
- `StatsRotation`

Each project received `Microsoft.Extensions.Hosting` 10.0.0. Existing application work remains in the entry points; the host now owns startup/shutdown lifetime through `Start`, `WaitForShutdown`, and disposal.

## Validation

- All six targeted .NET 10 executable projects built successfully individually.
- A complete solution build was run with `dotnet build WerewolfForTelegram.sln --no-restore`; it completed successfully with existing platform/legacy package warnings.
- The solution build reported 66 warnings and no errors. Existing warnings include CA1416 Windows API annotations and NU1701 legacy package compatibility warnings.

## Decision

No changes were required to `Languages/Languages.shproj`. The legacy .NET Framework web executables remain outside this Generic Host change and will be addressed during their planned migration.
