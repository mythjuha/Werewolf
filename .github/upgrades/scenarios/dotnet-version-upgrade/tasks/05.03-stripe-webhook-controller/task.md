# 05.03-stripe-webhook-controller: Port the Stripe webhook controller

## Objective
Port the Stripe webhook endpoint to ASP.NET Core.

## Scope
- Replace System.Web.Http and HttpResponseMessage APIs with ASP.NET Core controller APIs.
- Read Stripe settings through injected IConfiguration/options.
- Use WWCoreContext and EF Core persistence while preserving donation level and Telegram notification behavior.

## Done when
- The POST webhook route is mapped and compiles.
- Invalid signatures/events return appropriate 4xx responses.
- Secrets are not hard-coded.
