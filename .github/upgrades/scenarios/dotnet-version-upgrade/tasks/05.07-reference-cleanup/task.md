# 05.07-reference-cleanup: Validate DonationSite migration and clean up Core references

## Objective
Validate the migrated Core site and ensure the side-by-side boundary is clean.

## Scope
- Remove unused scaffold/proxy gaps and verify project references.
- Confirm no System.Web or EF6 references exist in DonationSite.Core.
- Build the Core project and affected dependencies; run available tests.
- Document runtime validation requiring the legacy site at its configured URL.
- Do not delete the legacy DonationSite project.

## Done when
- DonationSite.Core builds with zero warnings.
- Routes, static assets, and proxy fallback are validated as far as the environment permits.
- Legacy DonationSite remains in the solution for rollback.
