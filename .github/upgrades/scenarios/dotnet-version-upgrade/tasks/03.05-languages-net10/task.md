# 03.05-languages-net10: Handle Languages shared project for .NET 10

## Objective
Determine and implement the correct .NET 10-compatible handling for Languages/Languages.shproj and its consumers.

## Scope
- Inspect shared-project imports and consuming projects.
- Apply the minimum target-framework/project changes required.
- Restore/build affected consumers and document limitations.

## Research Findings

- `Languages/Languages.shproj` is a Visual Studio shared project containing shared XML language resources; it does not produce an independent assembly or target a framework.
- `Werewolf Node` loads the XML files at runtime from its `Languages` directory. Its existing post-build copy step remains responsible for deployment.
- The user separately requested Generic Host startup for all .NET 10 executables. That work was applied to Werewolf Control, Werewolf Node, Bot Watcher, Updater, ClearUpdates, and StatsRotation; it does not change the Languages shared project.
- Legacy .NET Framework web projects and `BuildAutomation` were intentionally excluded from the Generic Host change because they require their planned framework migration first.

## Outcome

No Languages project file changes are required. The shared project remains compatible with its existing consumers. The executable hosting changes are recorded in the scenario instructions and should be validated as a separate modernization concern.
