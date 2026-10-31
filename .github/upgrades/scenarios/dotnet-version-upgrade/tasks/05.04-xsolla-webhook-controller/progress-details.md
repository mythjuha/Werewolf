# Progress Details

## Changes
- Added the Core Xsolla event models required by the migrated webhook.
- Ported the Xsolla webhook to an ASP.NET Core API controller at `webhook/Xsolla`.
- Preserved signature verification, user validation, payment, refund, donation-level, badge, and GIF-unlock behavior.
- Replaced `ConfigurationManager`, `System.Web.Http`, `HttpResponseMessage`, `WWContext`, and EF6 persistence with Core configuration, controller results, `WWCoreContext`, and async EF Core operations.
- Kept Telegram and Xsolla secrets configuration-driven.

## Validation
- `DonationSite.Core.csproj`: build succeeded with no reported errors or warnings.
- Legacy DonationSite remains unchanged.

## Deferred
- Runtime webhook validation requires configured secrets and external Xsolla/Telegram callbacks.
