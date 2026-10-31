# Progress Details: 03.01-foundation-net10

## Changes
- Retargeted `Database`, `TcpFramework`, and `Shared` from `net472` to `net10.0`.
- Removed obsolete framework-included System.* PackageReferences and explicit framework assembly references from the three SDK-style projects.
- Updated `EntityFramework` in `Database` from 6.1.3 to 6.5.2, the latest stable supported package version identified for net10.0.
- Replaced `RNGCryptoServiceProvider` construction with `RandomNumberGenerator.Create()` in `Shared/GameBalancing.cs`.
- Annotated `Database/RegHelper.cs` with `[SupportedOSPlatform("windows")]` because it accesses the Windows registry.

## Validation
- `dotnet build TcpFramework.csproj`: passed with 0 warnings.
- `dotnet build Shared.csproj`: passed with 0 warnings.
- `dotnet build Database.csproj`: passed with 0 warnings after EntityFramework update.
- Outputs were generated under each project's `bin/Debug/net10.0` directory.

## Deferred Work
- Database still uses Entity Framework 6 and an EDMX model. EF6 compatibility is retained for now; a full EF Core/EDMX migration remains separate complex work if required by downstream runtime validation.
- Downstream projects still target Framework and must be migrated in subsequent dependency-ordered subtasks.
