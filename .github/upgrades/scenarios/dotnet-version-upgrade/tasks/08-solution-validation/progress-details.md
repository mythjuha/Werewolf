# Progress details

## Scope

Cleaned obsolete diagnostics only in .NET 10 projects. Legacy `Werewolf Website` and `DonationSite` Framework websites were not modified.

## Changes

- `Werewolf Control/Helpers/Bot.cs`: replaced obsolete `Assembly.CodeBase` path resolution with `Assembly.Location`.
- `Werewolf Control/Helpers/Extensions.cs`: replaced obsolete `RNGCryptoServiceProvider` with `RandomNumberGenerator.GetInt32` while preserving cryptographically secure shuffling.
- `Werewolf Control/Program.cs`: removed obsolete `ServicePointManager.SecurityProtocol` assignment; modern `HttpClient` does not use this global setting.
- `Werewolf Control/Helpers/LanguageHelper.cs`: replaced obsolete `WebClient.DownloadFile` with `HttpClient` byte download and file write.

## Validation

- Built the in-scope .NET 10 projects with `dotnet build --no-restore`.
- A follow-up scan across all in-scope project builds found zero `CS061*`, `SYSLIB*`, or obsolete diagnostics.
- `Werewolf Control` now has zero obsolete warnings, but retains unrelated pre-existing analyzer warnings (`CA1416`, `CA2200`, and `AsyncFixer03`) and builds with 0 errors.
- Existing non-obsolete compile errors remain in `ClearUpdates`, `StatsRotation`, and `Werewolf Node` around `ConfigurationManager.GetConnectionString`; these are outside the obsolete-warning cleanup request and were not changed.
- No test projects were available in the solution inventory.

## IConfiguration migration

Replaced all active `RegHelper.GetRegValue` calls in the .NET 10 Werewolf Control project with `Program.Configuration[...]` values loaded by `Host.CreateApplicationBuilder` from `appsettings.json`.

Changed files:
- `Werewolf for Telegram/Werewolf Control/Program.cs`
- `Werewolf for Telegram/Werewolf Control/Helpers/Updater.cs`
- `Werewolf for Telegram/Werewolf Control/Helpers/Bot.cs`
- `Werewolf for Telegram/Werewolf Control/Commands/GifCommands.cs`
- `Werewolf for Telegram/Werewolf Control/Commands/DevCommands.cs` (removed unused alias)
- `Werewolf for Telegram/Werewolf Control/appsettings.json`
- Removed `Werewolf for Telegram/Werewolf Control/Helpers/RegHelper.cs`

Configuration keys added include API tokens, Xsolla settings, Stripe settings, and the Azure DevOps token. The Telegram API token now uses the existing build configuration selection (`DebugAPI`, `ProductionAPI`, `ProductionAPI2`, or `BetaAPI`) through configuration instead of Windows Registry access.

Validation: `dotnet build Werewolf Control/WerewolfControl.csproj --no-restore` completed with 0 errors. Remaining warnings are pre-existing non-registry analyzer/compiler warnings; no `Microsoft.Win32`, registry, or active `RegHelper.GetRegValue` usages remain. Commented-out legacy CLI examples were not functional uses.
