# Progress Details

## Changes
- Added `DonationSite.Core/Controllers/HomeController.cs` using ASP.NET Core MVC.
- Ported the donation landing page to `DonationSite.Core/Views/Home/Index.cshtml`.
- Added Core MVC view infrastructure: `_ViewStart.cshtml`, `_ViewImports.cshtml`, shared layout, and error view.
- Copied the legacy Bootstrap, jQuery, and site stylesheet assets into `DonationSite.Core/wwwroot`.
- Added the Database project reference and mapped MVC/API controllers before the YARP fallback.

## Validation
- `DonationSite.Core.csproj`: build succeeded with no reported errors or warnings.
- Legacy DonationSite project was not modified or deleted.

## Deferred
- Xsolla and webhook controllers still need migration.
- Runtime verification requires starting the Core app and, for fallback routes, the legacy DonationSite at `http://localhost:52085`.
