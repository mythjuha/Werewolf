# .NET Version Upgrade

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: net10.0
- **Scope**: All projects in the solution

### Technical Preferences
- **Werewolf Control cache**: Replace `System.Runtime.Caching.MemoryCache` with `Microsoft.Extensions.Caching.Memory` and preserve one-hour expiration semantics.
- **Werewolf Control charting**: Replace `System.Windows.Forms.DataVisualization.Charting` with stable `ScottPlot.WinForms` and preserve PNG/JPEG Telegram chart output.
- **.NET 10 executable hosting**: Use `Host.CreateApplicationBuilder` and the Generic Host lifecycle for all .NET 10 executable projects; leave legacy .NET Framework executables for their later migration.
- **Obsolete-warning cleanup scope**: Inspect and modify only projects targeting .NET 10; ignore legacy .NET Framework websites.
- **BuildAutomation migration**: Proceed with a side-by-side .NET 10 Web API migration, keeping the existing .NET Framework project live until the replacement is validated.

## Source Control
- **Source Branch**: NetCore
- **Working Branch**: upgrade-dotnet-10 (new branch intended; Git was unavailable in the configured terminal)
- **Pending Changes**: Unable to verify because Git was unavailable
- **Commit Strategy**: After Each Task
- **Branch Sync**: Disabled until Git is available

## Upgrade Options
**Source**: .github/upgrades/scenarios/dotnet-version-upgrade/upgrade-options.md

### Strategy
- Upgrade Strategy: Bottom-Up

### Project Structure
- Project Approach: Side-by-side for ASP.NET Framework web projects; multi-targeting where required for dependent class libraries; in-place for System.Web-dependent libraries
- Package Management: Per-Project (defer CPM to post-migration)

### Compatibility
- Unsupported Packages: Defer Resolution
- Unsupported API Handling: Defer Complex Changes
- System.Web Adapters: Use System.Web Adapters
  Skill: aspnet-system-web-adapters

### Modernization
- Assembly Binding Redirects: Document and Review Before Removing

## Strategy
**Selected**: Bottom-Up (Dependency-First)
**Rationale**: The solution has multiple .NET Framework projects, legacy project formats, web applications, incompatible packages, and a multi-level dependency graph.

### Execution Constraints
- Convert non-web legacy projects to SDK-style before changing their target frameworks.
- Upgrade dependencies in topological order, validating each dependency group before moving upward.
- Side-by-side web scaffold tasks must complete and validate before their migration tasks begin.
- Keep old Framework web projects live and deployable throughout migration; do not delete them.
- Resolve simple compatibility changes inline, while tracking complex API and package resolutions for follow-up tasks.
- Run a full solution build and all available tests before final completion; defer central package management until the migration is stable.

### Side-by-Side Web Migration Constraints
- Scaffold task must complete and validate (builds, stub 200 response) before migrate starts.
- Old Framework project remains live and deployable throughout the entire migrate phase.
- Migrate tasks are broken into subtasks at execution time and use the ASP.NET Framework-to-Core migration guidance.
- Libraries in migrate task scope are handled in dependency order before web-layer assets.
- Reference cleanup is part of migrate tasks, not a separate task.
- Old projects are not deleted by the agent; removal is documented as a post-upgrade step for the user.
