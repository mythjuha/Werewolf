using System.Security.Claims;
using Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WerewolfWebsite.Core.Models;

namespace WerewolfWebsite.Core.Controllers;

[RequireHttps]
public sealed class AccountController(WWCoreContext database) : Controller
{
    private readonly PasswordHasher<AspNetUser> passwordHasher = new();

    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null) => View(new LoginViewModel { ReturnUrl = returnUrl });

    [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await database.AspNetUsers.Include(x => x.Roles)
            .SingleOrDefaultAsync(x => x.Email == model.Email, cancellationToken);
        if (user is null || string.IsNullOrEmpty(user.PasswordHash) ||
            passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        await SignInAsync(user, model.RememberMe);
        return RedirectToLocal(model.ReturnUrl);
    }

    [AllowAnonymous]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);
        if (await database.AspNetUsers.AnyAsync(x => x.Email == model.Email, cancellationToken))
        {
            ModelState.AddModelError(nameof(model.Email), "An account with this email already exists.");
            return View(model);
        }

        var user = new AspNetUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = model.Email,
            UserName = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            LockoutEnabled = true
        };
        user.PasswordHash = passwordHasher.HashPassword(user, model.Password);
        database.AspNetUsers.Add(user);
        await database.SaveChangesAsync(cancellationToken);
        await SignInAsync(user, false);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOff()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [AllowAnonymous]
    public IActionResult AccessDenied() => StatusCode(StatusCodes.Status403Forbidden);

    private async Task SignInAsync(AspNetUser user, bool isPersistent)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email ?? user.UserName)
        };
        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties { IsPersistent = isPersistent });
    }

    private IActionResult RedirectToLocal(string? returnUrl) =>
        !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? Redirect(returnUrl)
            : RedirectToAction("Index", "Home");
}
