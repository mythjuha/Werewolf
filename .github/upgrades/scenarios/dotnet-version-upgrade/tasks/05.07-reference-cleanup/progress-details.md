# Progress Details

## Changes
- Reviewed DonationSite.Core package and project references using the dependency graph.
- Confirmed the Core project references Database and uses ASP.NET Core MVC, System.Web Adapters, YARP, Stripe.net, and Telegram.Bot only.
- Pinned the transitive `System.Security.Cryptography.Pkcs` package to 10.0.14 in `Database.csproj`, resolving NU1902/NU1903 vulnerability warnings from version 9.0.11.
- Removed the platform-specific Windows Registry fallback from `WWCoreContext` so migrated web hosts use DI/configuration for the connection string.

## Validation
- `dotnet build DonationSite.Core/DonationSite.Core.csproj` passed with zero warnings and zero errors.
- Confirmed no source `System.Web`, EF6, legacy bundling, or `ConfigurationManager` usage remains in DonationSite.Core source files.
- Legacy DonationSite remains in the solution and was not deleted.

## Runtime notes
- Proxy fallback and external webhook/database behavior still require runtime configuration and service availability for full end-to-end verification.
