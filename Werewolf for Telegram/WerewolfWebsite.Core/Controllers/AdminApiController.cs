using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WerewolfWebsite.Core.Controllers;

[Authorize(Roles = "Developer"), RequireHttps]
public sealed class AdminApiController : Controller
{
    public IActionResult Index() => Content("Werewolf administration");

    public IActionResult Node(string id) => Content($"Node: {id}");

    public IActionResult Bot(string name) => Content($"Bot: {name}");

    public IActionResult Game(long groupid) => Content($"Game: {groupid}");

    public IActionResult Search(string? query) => Content($"Search: {query}");

    [HttpGet]
    public IActionResult GetStatus() => Ok(Array.Empty<object>());

    [HttpGet]
    public IActionResult GetBotStatus(string botname) => NotFound(new { botname });

    [HttpGet]
    public IActionResult GetNodeInfo(string clientid) => NotFound(new { clientid });

    [HttpGet]
    public IActionResult GetGameInfo(long groupid, string clientid) => NotFound(new { groupid, clientid });

    [HttpPost]
    public IActionResult StopNode(string id, string ip, int port) => Accepted(new { id, ip, port });
}
