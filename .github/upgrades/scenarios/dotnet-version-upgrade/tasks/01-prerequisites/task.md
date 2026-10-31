# 01-prerequisites: Verify the .NET 10 upgrade toolchain

Verify that the .NET 10 SDK, required Visual Studio/MSBuild components, and any `global.json` constraints support the target framework. Establish the baseline build/test status and record the available test projects before changing project files.

## Research Findings

- **Solution**: `Werewolf for Telegram/WerewolfForTelegram.sln`.
- **Scope**: 12 projects were assessed; the solution also contains `Languages/Languages.shproj`.
- **SDK**: `validate_dotnet_sdk_installation(net10.0)` reported a compatible SDK.
- **SDK pinning**: No `global.json` was found, so no repository SDK constraint requires adjustment.
- **Shared MSBuild files**: No `Directory.Build.props` or `Directory.Build.targets` were found by file search.
- **Baseline build**: The Visual Studio workspace build completed successfully before migration changes.
- **Tests**: Test-project discovery returned no test projects for the solution scope.
- **Build approach**: Because the solution currently contains legacy non-SDK-style projects and .NET Framework targets, use Visual Studio MSBuild for legacy project validation; use `dotnet build` after projects become SDK-style and target modern .NET.

**Done when**: The .NET 10 SDK and repository SDK configuration are compatible, baseline validation is recorded, and the solution's upgrade prerequisites are satisfied or documented as blockers.
