# 05-migrate-donationsite: Incrementally migrate DonationSite to ASP.NET Core

Migrate the DonationSite web surface from System.Web to the scaffolded ASP.NET Core project while using System.Web Adapters where needed. Address Global.asax initialization, bundling/minification replacement, configuration, DI, controllers/views, middleware, package compatibility, and the shared-library references used by the site. Keep the Framework application live until the Core implementation is validated.

## Baseline and research findings

- Source project: `Werewolf for Telegram/DonationSite` (`.NET Framework 4.7.2`, ASP.NET MVC/Web API).
- Target project: `Werewolf for Telegram/DonationSite.Core` (`net10.0`, ASP.NET Core MVC/YARP proxy).
- Legacy MVC routes use the default `{controller}/{action}/{id}` route with `Home/Index` defaults.
- Legacy Web API routes use attribute routing plus `webhook/{controller}/{id}`; Stripe and Xsolla webhook actions are POST endpoints.
- Legacy controllers: `HomeController`, `XsollaController`, `Webhook/StripeController`, and `Webhook/XsollaController`.
- Legacy views: `Home/Index`, `Xsolla/Index`, `Shared/_Layout`, `Shared/Error`, and `_ViewStart`.
- Legacy static assets are under `Content/` and `Scripts/`, including Bootstrap, jQuery, and `Site.css`; they must be copied to `DonationSite.Core/wwwroot` and referenced directly rather than through `System.Web.Optimization`.
- Legacy configuration contains non-secret MVC settings in `Web.config`; webhook secrets and Telegram settings are supplied through `AppSettingsSecrets.config` and must be represented as environment/appsettings configuration without committing secret values.
- `XsollaController` reads `Database.WWContext` and must use the active `WWCoreContext`/EF Core model in the Core project.
- Webhook controllers use `ConfigurationManager`, `System.Web.Http`, `HttpRequestMessage`, and EF6 `SaveChanges`; these require ASP.NET Core controller actions, injected configuration, `HttpRequest`, and EF Core context changes.
- Existing Core host has MVC and YARP forwarding, but no migrated controllers, views, static assets, project references, or `MapControllers()` endpoint mapping.

## Migration units

1. `05.01-home-controller`: port anonymous Home MVC endpoint and view.
2. `05.02-xsolla-page-controller`: port Xsolla donation page and EF Core lookup.
3. `05.03-stripe-webhook-controller`: port Stripe webhook endpoint and configuration/Telegram integration.
4. `05.04-xsolla-webhook-controller`: port Xsolla webhook endpoint and signature/payment handling.
5. `05.05-pipeline-and-configuration`: migrate routes, configuration, DI, error handling, and middleware.
6. `05.06-views-and-static-assets`: port layouts, views, Bootstrap/jQuery assets, and CSS.
7. `05.07-reference-cleanup`: remove obsolete proxy-only gaps, verify Core project references and legacy-project preservation.

**Done when**: DonationSite functionality is migrated and validated on .NET 10, adapter usage is limited to required compatibility seams, routes and static assets work, tests/builds pass, and the legacy project remains available for rollback and post-production removal.
