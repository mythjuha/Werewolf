# 03.03-werewolf-node-net10: Migrate Werewolf Node to .NET 10

## Objective
Migrate Werewolf Node to net10.0 after its foundation dependencies are stable.

## Scope
- Update target framework and package references.
- Resolve simple API, cryptography, and assembly-binding issues.
- Preserve Telegram.Bot behavior and wildcard assembly versioning where needed.
- Restore and build, documenting deferred incompatibilities.

## Research Findings

- `WerewolfNode.csproj` is SDK-style, currently targets `net472`, outputs an x64 executable in Debug, preserves wildcard assembly versions with `Deterministic=false`, and has project references to Database, Shared, and TcpFramework—all now targeting net10.0.
- The project contains a large legacy generated System.* package inventory, explicit framework references, BotanIO.Net 1.1.0, EntityFramework 6.1.3, Newtonsoft.Json 13.0.1, System.Net.Http.Formatting.Extension 5.2.3, and Telegram.Bot 19.0.0-preview.1.
- Assessment reports one incompatible package, 7 source API findings, 3 behavioral API findings, cryptography concerns, and binding redirect issues. BotanIO.Net and System.Net.Http.Formatting.Extension are the principal unsupported/deferred package risks.
- Source search found no direct `System.Security.Cryptography` usage in the project source; the cryptography findings are package/transitive or shared dependency related. No CodeBase or ServicePointManager usage was found in the project source search.
- The existing post-build xcopy copies language XML files from the solution Languages folder; preserve it unless net10 build proves it incompatible.
- Package management is per-project; no CPM was detected.

## Execution Decisions

- Replace the target with `net10.0`; preserve x64 Debug, custom configurations, assembly name, and `Deterministic=false`.
- Remove framework-included System.* PackageReferences and explicit framework assembly references after confirming source usage, but retain genuine third-party packages during the first compatibility build.
- Align EntityFramework with Database at 6.5.2 and update Newtonsoft.Json to 13.0.4 if the current API surface remains compatible.
- Do not replace BotanIO.Net, System.Net.Http.Formatting.Extension, or Telegram.Bot in this task without a compile/runtime-preserving alternative; document them as deferred if unsupported.
- Build with restore enabled and fix all warnings where the required change is mechanical. Do not suppress warnings.

## Validation

1. Restore/build the net10.0 project and all three net10 project references.
2. Confirm output is under `bin/.../net10.0` and the language-file post-build step remains valid.
3. Record package/API issues that cannot be safely resolved in `progress-details.md`.
