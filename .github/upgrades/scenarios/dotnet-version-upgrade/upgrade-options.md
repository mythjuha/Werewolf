# Upgrade Options — WerewolfForTelegram

Assessment: 12-project .NET Framework solution crossing to .NET 10, with legacy project formats, ASP.NET Framework web projects, incompatible/deprecated/vulnerable packages, binding redirects, and API compatibility findings.

## Strategy

### Upgrade Strategy
.NET Framework projects and multiple project dependencies require dependency-first migration with tier validation.

| Value | Description |
|-------|-------------|
| **Bottom-Up** (selected) | Upgrade leaf projects first, then move upward through the dependency graph with validation after each tier. |

## Project Structure

### Project Approach
The solution contains ASP.NET Framework web projects and shared class libraries that must support the transition while the web applications are migrated.

| Value | Description |
|-------|-------------|
| **Side-by-side** (selected) | Create ASP.NET Core projects alongside the existing web projects and migrate incrementally while the old applications remain available. Class libraries use multi-targeting where required during the transition. |
| In-place rewrite | Replace each ASP.NET Framework web project in one pass. |

### Package Management
The migration crosses the .NET Framework boundary and includes legacy project formats, so central package management would add churn during the transition.

| Value | Description |
|-------|-------------|
| **Per-Project (defer CPM to post-migration)** (selected) | Keep package versions in each project during migration and consider central package management after all projects stabilize on .NET 10. |
| Central Package Management | Create Directory.Packages.props and centralize package versions during the migration. |

## Compatibility

### Unsupported Packages
The assessment identifies incompatible packages across multiple projects, and the dependency-first strategy prioritizes buildable tiers.

| Value | Description |
|-------|-------------|
| **Defer Resolution** (selected) | Keep projects compilable with minimal temporary stubs or conditional references, then resolve incompatible packages in follow-up work. |
| Resolve Inline | Research and replace each incompatible package within the current upgrade task. |
| Compatibility Mode | Retain selected Framework package references using compatibility assemblies where safe. |

### Unsupported API Handling
API incompatibility and behavioral-change findings span multiple projects, making a staged resolution safer during tier migration.

| Value | Description |
|-------|-------------|
| **Defer Complex Changes** (selected) | Apply mechanical replacements immediately and defer complex API rewrites behind compilable temporary stubs for dedicated follow-up tasks. |
| Fix Inline | Resolve all API changes, including complex changes, within the current task. |

### System.Web Adapters
The solution contains ASP.NET Framework/System.Web web projects and the selected side-by-side approach benefits from compatibility shims during incremental migration.

| Value | Description |
|-------|-------------|
| **Use System.Web Adapters** (selected) | Add System.Web adapter support for incremental migration, then remove shims during final cleanup. |
| Direct Migration to ASP.NET Core APIs | Replace System.Web usage directly with native ASP.NET Core APIs without adapter shims. |

## Modernization

### Assembly Binding Redirects
Binding redirects are present, including partial manual redirects and forced version downgrades, so they require review rather than blind removal.

| Value | Description |
|-------|-------------|
| **Document and Review Before Removing** (selected) | Inventory redirects and resolve underlying package conflicts before removing legacy redirect configuration. |
| Remove Binding Redirects | Remove binding redirects because modern .NET resolves assemblies without Framework redirect configuration. |
