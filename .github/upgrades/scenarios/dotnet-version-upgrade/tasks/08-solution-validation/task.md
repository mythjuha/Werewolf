# 08-solution-validation: Validate the complete .NET 10 solution

Run targeted validation for the .NET 10 projects and fix obsolete API warnings without modifying the retained .NET Framework websites. The current user scope supersedes the original solution-wide warning criterion for this operation.

## Research findings

- Modern project inventory: `Bot Watcher/Bot Watcher.csproj` (`net10.0`), `ClearUpdates/ClearUpdates.csproj` (`net10.0-windows`), `Database/Database.csproj` (`net10.0`), `DonationSite.Core/DonationSite.Core.csproj` (`net10.0`), `Shared/Shared.csproj` (`net10.0`), `StatsRotation/StatsRotation.csproj` (`net10.0`), `TcpFramework/TcpFramework.csproj` (`net10.0`), `Updater/Updater.csproj` (`net10.0`), `Werewolf Control/WerewolfControl.csproj` (`net10.0-windows`), `Werewolf Node/WerewolfNode.csproj` (`net10.0-windows`), and `WerewolfWebsite.Core/WerewolfWebsite.Core.csproj` (`net10.0`).
- Explicitly excluded: legacy `Werewolf Website` and `DonationSite` Framework websites, plus any other project not targeting .NET 10.
- `WerewolfControl.csproj` is SDK-style, Windows-targeted, uses WinForms and ScottPlot, and references Database, Shared, and TcpFramework; it should be validated with Visual Studio MSBuild if the SDK build encounters Windows/resource limitations.
- Initial source search found modern candidates in `Werewolf Control/Program.cs` (`ServicePointManager.SecurityProtocol`), `Werewolf Control/Helpers/LanguageHelper.cs` (`WebClient`), and `Werewolf Control/Helpers/Extensions.cs` (`RNGCryptoServiceProvider`). Exact compiler/analyzer diagnostics must drive edits; no warning suppression is permitted.
- `WerewolfWebsite.Core` previously built with zero warnings/errors; it still requires targeted confirmation.

## Execution plan

1. Build each .NET 10 project and capture obsolete diagnostics (`CS0612`, `CS0618`, `SYSLIB*`, and analyzer obsolete diagnostics).
2. Fix obsolete APIs only in the affected .NET 10 project sources, preserving behavior.
3. Rebuild touched projects warning-free and run available tests.
4. Record results in `progress-details.md`; legacy Framework warnings remain intentionally outside scope.

**Done when**: Every in-scope .NET 10 project has no obsolete warnings, touched projects build successfully, available tests pass, and excluded Framework projects are not modified.
