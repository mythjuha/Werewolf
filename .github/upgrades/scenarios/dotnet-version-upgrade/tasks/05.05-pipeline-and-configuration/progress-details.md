# Progress Details

## Changes
- Registered `WWCoreContext` with ASP.NET Core DI and the `BotConnectionString` configuration key.
- Migrated the legacy default route to `MapControllerRoute` and mapped attribute-routed webhooks before the YARP fallback.
- Added production exception handling and preserved development diagnostics.
- Added the non-secret connection-string configuration shape and explicit development proxy target.
- Kept the legacy DonationSite proxy target and legacy project intact.

## Validation
- `DonationSite.Core.csproj`: build succeeded with no reported errors or warnings.
- No secrets were added to committed configuration.

## Deferred
- Actual database and webhook runtime validation requires environment-provided connection strings and API secrets.
