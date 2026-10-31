using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DonationSite.Core.Controllers;

public sealed class XsollaController : Controller
{
    private readonly WWCoreContext _db;

    public XsollaController(WWCoreContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? uid = null, string? token = null)
    {
        ViewData["Title"] = "Xsolla Donation";

        if (string.IsNullOrWhiteSpace(uid) || string.IsNullOrWhiteSpace(token) ||
            !long.TryParse(uid, out var userId))
        {
            return RedirectToAction("Index", "Home");
        }

        var user = await _db.Players.FirstOrDefaultAsync(player => player.TelegramId == userId);
        if (user is null)
        {
            return RedirectToAction("Index", "Home");
        }

        var model = new XsollaDonationViewModel(user.Name, user.TelegramId, token);
        return View(model);
    }
}

public sealed record XsollaDonationViewModel(string? Name, long TelegramId, string AccessToken);
