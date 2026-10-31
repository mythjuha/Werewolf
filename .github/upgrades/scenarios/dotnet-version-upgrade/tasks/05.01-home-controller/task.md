# 05.01-home-controller: Port the anonymous Home MVC endpoint and view

## Objective
Port DonationSite HomeController and its anonymous donation landing page to DonationSite.Core.

## Scope
- Add the ASP.NET Core HomeController.
- Port Home/Index.cshtml and its model-free donation links.
- Preserve the default Home/Index route and verify the endpoint builds.

## Done when
- HomeController compiles in DonationSite.Core.
- Home/Index is available to the Core MVC pipeline.
- The legacy DonationSite project remains unchanged and available as rollback.
