# 05.05-pipeline-and-configuration: Migrate DonationSite pipeline, routes, configuration, and DI

## Objective
Complete the ASP.NET Core host configuration needed by migrated DonationSite endpoints.

## Scope
- Migrate relevant appSettings and connection strings without copying secrets.
- Register WWCoreContext, Telegram/webhook dependencies, and configuration options.
- Add controller routing, default MVC routing, error handling, and required middleware.
- Keep YARP fallback to the legacy site for unmigrated requests.

## Done when
- Core host starts and resolves controller dependencies.
- MVC and webhook routes are mapped before the proxy fallback.
- Legacy DonationSite remains configured as the fallback target.
