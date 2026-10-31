using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Database;
using Microsoft.EntityFrameworkCore;

namespace WerewolfWebsite.Core.Controllers;

[AllowAnonymous]
public sealed class HomeController(WWCoreContext database) : Controller
{
    public IActionResult Index()
    {
        SetMetadata("Home", "Werewolf for Telegram. Play werewolf while chatting with your friends on Telegram!");
        return View();
    }

    [HttpGet]
    public IActionResult GetBotStatus() => Ok(Array.Empty<string>());

    [HttpGet]
    public async Task<IActionResult> GetPublicGroups(CancellationToken cancellationToken)
    {
        var groups = await database.Groups.AsNoTracking()
            .Where(x => x.Preferred == true && x.UserName != null && x.UserName != string.Empty)
            .Select(x => new
            {
                x.Name,
                x.Language,
                x.UserName,
                Link = "http://telegram.me/" + x.UserName
            })
            .ToListAsync(cancellationToken);
        return Ok(groups);
    }

    public IActionResult About()
    {
        SetMetadata("About", "Information about Werewolf for Telegram");
        return View();
    }

    public IActionResult Contact()
    {
        SetMetadata("Contact", "Contact information");
        return View();
    }

    public IActionResult Achievements()
    {
        SetMetadata("Werewolf Telegram - Achievements", null);
        return View();
    }

    private void SetMetadata(string title, string? description)
    {
        ViewData["Title"] = title;
        ViewData["Description"] = description ?? "Werewolf for Telegram. Play werewolf while chatting with your friends on Telegram!";
    }
}
