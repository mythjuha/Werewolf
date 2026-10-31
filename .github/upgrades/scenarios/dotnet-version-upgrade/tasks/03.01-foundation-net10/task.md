# 03.01-foundation-net10: Migrate foundation libraries to .NET 10

## Objective
Migrate Database, TcpFramework, and Shared from their Framework targets to net10.0 in dependency order.

## Scope
- Update target frameworks and package references as required.
- Apply simple compatibility fixes, especially Shared cryptography findings.
- Restore and build all three projects before proceeding.
- Document deferred package/API issues.

## Dependencies
These are leaf/foundation projects used by later applications.

## Research Findings

- Repository uses per-project package references; `get_project_dependencies` found no Central Package Management imports for any of the three projects.
- `Database/Database.csproj` is SDK-style, currently targets `net472`, references EntityFramework 6.1.3, and contains old System.IO/System.Net.Http/System.Runtime/System.Security.Cryptography package references plus explicit framework assembly references. It contains an EDMX/EntityDeploy model and generated T4 code, so full Visual Studio MSBuild may be required if SDK build resource/model tasks fail.
- `TcpFramework/TcpFramework.csproj` is SDK-style, currently targets `net472`, and has only framework-included System.* package references plus explicit Microsoft.CSharp, DataSetExtensions, and Composition references.
- `Shared/Shared.csproj` is SDK-style, currently targets `net472`, and has the same legacy System.* package references. `GameBalancing.cs` uses `RNGCryptoServiceProvider`; this is the only direct cryptography API match found and should be replaced with the modern static `RandomNumberGenerator` API if the current call pattern permits.
- Existing project references consume these libraries, so each project must restore/build successfully before dependent task `03.02` starts.
- No Directory.Build.props or Directory.Packages.props package management was detected by dependency inspection.

## Execution Decisions

- Replace the single `TargetFramework` values in all three projects with `net10.0`; do not multi-target unless a downstream compatibility error proves it necessary.
- Remove obsolete framework-included System.* package references only after the first net10 restore identifies whether they are still needed. Keep EntityFramework initially because Database contains EDMX/EF6 generated code; if EF6 is incompatible with net10, document/defer the EF6-to-EF Core migration rather than rewriting the EDMX in this task.
- Preserve existing assembly metadata and generated-code item configuration unless the net10 build requires a focused change.
- Use targeted restore/build for each foundation project, then a dependency-group build gate. Treat warnings as errors for remediation and do not suppress them.

## Validation

1. Restore/build `TcpFramework` and `Shared` on `net10.0`.
2. Restore/build `Database` on `net10.0`; escalate to Visual Studio MSBuild for EDMX/resource tooling failures.
3. Confirm outputs are under `bin/.../net10.0` and no project still targets `net472`.
4. Record all changed files, build results, and deferred package/API issues in `progress-details.md` before completing the subtask.
