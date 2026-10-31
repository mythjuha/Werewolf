# 05.06-views-and-static-assets: Port DonationSite views and static assets

## Objective
Port the legacy Razor views, layout, CSS, Bootstrap, and jQuery assets to DonationSite.Core.

## Scope
- Copy and adapt Home/Index, Xsolla/Index, shared layout/error views, and _ViewStart.
- Copy Content and Scripts into wwwroot.
- Replace Scripts.Render/Styles.Render with direct static asset references and Tag Helpers.

## Done when
- Home and Xsolla views render in ASP.NET Core.
- Static assets load from wwwroot without System.Web.
- No Scripts.Render or Styles.Render references remain in the Core project.
