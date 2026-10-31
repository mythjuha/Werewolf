# Progress Details

## Changes
- Ported DonationSite views and shared layout to DonationSite.Core.
- Added `_ViewImports.cshtml` with ASP.NET Core MVC Tag Helpers and `_ViewStart.cshtml`.
- Replaced legacy `Scripts.Render`/`Styles.Render` bundle references with direct static file links.
- Copied Bootstrap, jQuery, and site CSS assets into `wwwroot/css` and `wwwroot/js`.
- Confirmed `Program.cs` enables static files before routing.

## Validation
- DonationSite.Core build succeeded after the view and asset migration.
- Search confirmed no `Scripts.Render`, `Styles.Render`, or `System.Web` references remain in the Core views.
- Legacy DonationSite views and assets remain intact.

## Deferred
- Runtime browser verification remains pending until the Core host and its configured dependencies are started.
