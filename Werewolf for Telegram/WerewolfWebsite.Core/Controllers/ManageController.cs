using Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WerewolfWebsite.Core.Models;

namespace WerewolfWebsite.Core.Controllers;

[Authorize, RequireHttps]
public sealed class ManageController(WWCoreContext database) : Controller
{
    private readonly PasswordHasher<AspNetUser> passwordHasher = new();

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var user = await CurrentUser(cancellationToken);
        if (user is null)
            return Challenge();
        return View(new ManageAccountViewModel
        {
            HasPassword = !string.IsNullOrEmpty(user.PasswordHash),
            PhoneNumber = user.PhoneNumber,
            TwoFactorEnabled = user.TwoFactorEnabled
        });
    }

    public IActionResult ChangePassword() => View(new ChangePasswordViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);
        var user = await CurrentUser(cancellationToken);
        if (user is null || string.IsNullOrEmpty(user.PasswordHash) ||
            passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.OldPassword) == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError(nameof(model.OldPassword), "The current password is incorrect.");
            return View(model);
        }

        user.PasswordHash = passwordHasher.HashPassword(user, model.NewPassword);
        user.SecurityStamp = Guid.NewGuid().ToString();
        await database.SaveChangesAsync(cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    private Task<AspNetUser?> CurrentUser(CancellationToken cancellationToken) =>
        database.AspNetUsers.SingleOrDefaultAsync(x => x.Id == User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value, cancellationToken);
}
