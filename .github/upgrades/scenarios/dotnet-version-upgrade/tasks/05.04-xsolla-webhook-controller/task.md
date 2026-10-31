# 05.04-xsolla-webhook-controller: Port the Xsolla webhook controller

## Objective
Port the Xsolla webhook endpoint to ASP.NET Core.

## Scope
- Preserve signature verification, user validation, payment, and refund handling.
- Replace System.Web.Http request/response APIs and ConfigurationManager.
- Use WWCoreContext and Core-compatible Telegram/JSON APIs.

## Done when
- The webhook route is mapped and compiles.
- Signature and invalid-user responses preserve the legacy contract.
- Secrets are configuration-driven.
