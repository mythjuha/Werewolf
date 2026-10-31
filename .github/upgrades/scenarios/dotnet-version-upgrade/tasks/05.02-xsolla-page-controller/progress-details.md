# Progress Details

## Changes
- Added the ASP.NET Core `XsollaController`.
- Replaced the legacy `WWContext` lookup with injected `WWCoreContext` and asynchronous EF Core access.
- Preserved redirect behavior for invalid/missing user parameters.
- Added a strongly typed Xsolla donation view model and ported the Xsolla donation view.
- Serialized the Xsolla access token safely into the page instead of reading it from the URL in client script.

## Validation
- `DonationSite.Core.csproj`: build succeeded with no reported errors or warnings.
- Legacy DonationSite remains unchanged.

## Deferred
- Stripe and Xsolla webhook controllers still require migration.
