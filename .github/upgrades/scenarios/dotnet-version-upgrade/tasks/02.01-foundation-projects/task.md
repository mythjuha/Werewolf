# 02.01-foundation-projects: Convert leaf non-web projects to SDK-style

## Objective
Convert the leaf non-web legacy projects to SDK-style without changing target frameworks or package versions.

## Scope
Database, TcpFramework, Shared, Bot Watcher, BuildAutomation, and Updater. Preserve packages.config dependencies through the conversion tool and validate each project sequentially using the topological order.

## Steps
1. Convert one project at a time with convert_project_to_sdk_style.
2. Build each converted project on its original .NET Framework target with Visual Studio MSBuild.
3. Verify packages.config migration and preserve project/resource metadata.
4. Record any conversion-specific warnings or blockers before proceeding.

## Done when
All six foundation projects are SDK-style, retain their original target frameworks and package versions, and build without warnings or errors.
