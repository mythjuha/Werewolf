# Progress Details: 04-scaffold-donationsite

## Changes
- Scaffolded `DonationSite.Core` as a sibling ASP.NET Core MVC project targeting `net10.0`.
- Added the new project to `WerewolfForTelegram.sln`.
- Configured YARP and System.Web Adapters in `Program.cs`.
- Configured the proxy target as `http://localhost:52085`, matching the legacy DonationSite IIS Express URL.
- Updated `Yarp.ReverseProxy` from 2.0.1 to the supported 2.3.0 package version.
- The legacy DonationSite project remains in the solution and was not deleted or disabled.

## Validation
- `DonationSite.Core.csproj` builds successfully.
- The new project contains the YARP catch-all forwarding route and System.Web Adapter registration.
- Runtime proxy validation still requires the legacy DonationSite application to be running at `http://localhost:52085`.

## Deferred Work
- Migrate DonationSite controllers, views, static assets, configuration, and EF6 data access into the new ASP.NET Core project.
- Replace or isolate the legacy Framework DonationSite EF6 references during the migration task.
