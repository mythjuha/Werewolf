# 04-scaffold-donationsite: Scaffold the ASP.NET Core DonationSite proxy

Create a new ASP.NET Core `net10.0` project alongside the existing `DonationSite` Framework application and configure the YARP reverse proxy for side-by-side operation. Preserve the existing application as the fallback/live implementation while the new project establishes a buildable Core host and a validated proxy route.

**Done when**: The new DonationSite Core project is in the solution, restores and builds on .NET 10, routes a known request through YARP to the existing application, and the old project remains deployable.
