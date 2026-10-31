using Microsoft.AspNetCore.Mvc;

namespace DonationSite.Core.Controllers;

public sealed class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Donate";
        return View();
    }
}
