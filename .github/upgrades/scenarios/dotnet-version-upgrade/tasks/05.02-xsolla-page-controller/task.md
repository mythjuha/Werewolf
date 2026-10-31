# 05.02-xsolla-page-controller: Port the Xsolla donation page controller

## Objective
Port the Xsolla donation page to ASP.NET Core MVC using WWCoreContext.

## Scope
- Port XsollaController and its Index action.
- Replace WWContext/EF6 access with WWCoreContext/EF Core.
- Port the Xsolla view and preserve redirect behavior for invalid requests.

## Done when
- Valid and invalid Xsolla page requests have equivalent behavior.
- DonationSite.Core builds without warnings.
