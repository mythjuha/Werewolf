using System.Collections;
using Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WerewolfWebsite.Core.Controllers;

[AllowAnonymous]
public sealed class StatsController(WWCoreContext database, IConfiguration configuration) : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Werewolf Telegram Stats";
        ViewData["Description"] = "Werewolf for Telegram. Play werewolf while chatting with your friends on Telegram! Werewolf for Telegram Stats";
        return View();
    }

    public async Task<IActionResult> Group(long id, CancellationToken cancellationToken)
    {
        if (!HasDatabaseConnection())
        {
            ViewData["Title"] = "Group Stats";
            ViewData["Description"] = "Werewolf for Telegram group statistics";
            ViewData["Id"] = id;
            return View();
        }

        var group = await database.Groups.AsNoTracking().FirstOrDefaultAsync(x => x.GroupId == id, cancellationToken);
        ViewData["Title"] = group?.Name ?? "Invalid Group Id";
        ViewData["Description"] = $"Werewolf for Telegram. Stats for {ViewData["Title"]}";
        ViewData["Id"] = id;
        return View();
    }

    public async Task<IActionResult> Player(long id, bool json = false, CancellationToken cancellationToken = default)
    {
        if (!HasDatabaseConnection())
        {
            if (json)
            {
                return DatabaseUnavailable();
            }

            ViewData["Title"] = "Player Stats";
            ViewData["Description"] = "Werewolf for Telegram player statistics";
            ViewData["ImagePath"] = "/images/noimage.png";
            ViewData["Id"] = id;
            return View();
        }

        var player = await database.Players.AsNoTracking().FirstOrDefaultAsync(x => x.TelegramId == id, cancellationToken);
        if (json)
        {
            if (player is null)
            {
                return NotFound();
            }

            return new JsonResult(new
            {
                id = player.Id,
                telegramId = player.TelegramId,
                name = player.Name,
                username = player.UserName,
                language = player.Language
            });
        }

        ViewData["Title"] = player?.Name ?? "Invalid User Id";
        ViewData["Description"] = $"Werewolf for Telegram. Stats for {ViewData["Title"]}";
        ViewData["ImagePath"] = "/images/" + (player?.ImageFile ?? "noimage.png");
        ViewData["Id"] = id;
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GlobalStats(bool json = false, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(configuration.GetConnectionString("BotConnectionString")))
        {
            return Problem("Statistics are temporarily unavailable because the database connection is not configured.", statusCode: StatusCodes.Status503ServiceUnavailable);
        }

        var stat = await database.GlobalStats.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        if (stat is null)
        {
            return NotFound();
        }

        var night1Link = $"<a href='/Stats/Player/{stat.MostKilledFirstNightId}'>{stat.MostKilledFirstNight}</a>";
        var day1LynchLink = $"<a href='/Stats/Player/{stat.MostLynchedFirstDayId}'>{stat.MostLynchedFirstDay}</a>";
        var day1DeathLink = $"<a href='/Stats/Player/{stat.MostKilledFirstDayId}'>{stat.MostKilledFirstDay}</a>";
        var survivorLink = $"<a href='/Stats/Player/{stat.BestSurvivorId}'>{stat.BestSurvivor}</a>";

        if (json)
        {
            return new JsonResult(new
            {
                gamesPlayed = stat.GamesPlayed,
                playersKilled = stat.PlayersKilled,
                playersSurvived = stat.PlayersSurvived,
                totalPlayers = stat.TotalPlayers,
                totalGroups = stat.TotalGroups,
                mostKilledFirstNight = new { name = stat.MostKilledFirstNight, id = stat.MostKilledFirstNightId, link = night1Link, percent = stat.MostKilledFirstPercent },
                mostLynchedFirstDay = new { name = stat.MostLynchedFirstDay, id = stat.MostLynchedFirstDayId, link = day1LynchLink, percent = stat.MostLynchedFirstPercent },
                mostKilledFirstDay = new { name = stat.MostKilledFirstDay, id = stat.MostKilledFirstDayId, link = day1DeathLink, percent = stat.MostKilledFirstDayPercent },
                bestSurvivor = new { name = stat.BestSurvivor, id = stat.BestSurvivorId, link = survivorLink, percent = stat.BestSurvivorPercent },
                lastCalculated = stat.LastRun?.ToString()
            });
        }

        var html = $"<table class=\"table table-hover\"><tbody>" +
            $"<tr><td>Games played total</td><td><b>{stat.GamesPlayed}</b></td></tr>" +
            $"<tr><td>Total player deaths</td><td><b>{stat.PlayersKilled}</b></td></tr>" +
            $"<tr><td>Total player survivals</td><td><b>{stat.PlayersSurvived}</b></td></tr>" +
            $"<tr><td>Total players in database</td><td><b>{stat.TotalPlayers}</b></td></tr>" +
            $"<tr><td>Total groups in database</td><td><b>{stat.TotalGroups}</b></td></tr>" +
            $"<tr><td>Most likely to die first night</td><td><b>{night1Link}</b></td><td>{stat.MostKilledFirstPercent}%</td></tr>" +
            $"<tr><td>Most likely to get lynched first day</td><td><b>{day1LynchLink}</b></td><td>{stat.MostLynchedFirstPercent}%</td></tr>" +
            $"<tr><td>Most likely to die first 24 hours</td><td><b>{day1DeathLink}</b></td><td>{stat.MostKilledFirstDayPercent}%</td></tr>" +
            $"<tr><td>Best survivor</td><td><b>{survivorLink}</b></td><td>{stat.BestSurvivorPercent}%</td></tr>" +
            $"<tr><td>Last time global stats calculated</td><td><b>{stat.LastRun} (Central US)</b></td></tr></tbody></table>";
        return new JsonResult(html);
    }

    [HttpGet]
    public async Task<IActionResult> DailyCounts(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(configuration.GetConnectionString("BotConnectionString")))
        {
            return Problem("Statistics are temporarily unavailable because the database connection is not configured.", statusCode: StatusCodes.Status503ServiceUnavailable);
        }

        var counts = await database.DailyCounts.AsNoTracking()
            .OrderByDescending(x => x.Day)
            .Take(30)
            .ToListAsync(cancellationToken);
        return new JsonResult(counts.OrderBy(x => x.Day).Select(x => new { Day = x.Day.Day.ToString(), x.Groups, x.Games, Users = x.Users }));
    }

    [HttpGet]
    public async Task<IActionResult> GroupStats(long groupid, bool json = false, CancellationToken cancellationToken = default)
    {
        if (!HasDatabaseConnection())
        {
            return DatabaseUnavailable();
        }

        var survivor = (await database.GroupSurvivor(groupid).AsNoTracking().ToListAsync(cancellationToken)).FirstOrDefault();
        var gamesPlayed = await database.Games.CountAsync(x => x.GroupId == groupid, cancellationToken);
        var survivorLink = survivor is null
            ? "Not enough games"
            : $"<a href='../Player/{survivor.TelegramId}'>{survivor.Name}</a>";

        if (json)
        {
            return new JsonResult(new
            {
                gamesPlayedTotal = gamesPlayed,
                bestSurvivor = new
                {
                    name = survivor?.Name ?? "Not enough games",
                    id = survivor?.TelegramId ?? 0,
                    link = survivorLink,
                    survivedPercent = survivor?.pct ?? 0
                }
            });
        }

        var html = $"<table class=\"table table-hover\"><tbody>" +
            $"<tr><td>Games played total</td><td><b>{gamesPlayed}</b></td></tr>" +
            $"<tr><td>Best survivor</td><td><b>{survivorLink}</b></td><td>{survivor?.pct ?? 0}%</td></tr>" +
            "</tbody></table>";
        return new JsonResult(html);
    }

    [HttpGet]
    public async Task<IActionResult> PlayerStats(long pid, bool json = false, CancellationToken cancellationToken = default)
    {
        if (!HasDatabaseConnection())
        {
            return DatabaseUnavailable();
        }

        var player = await database.Players.AsNoTracking().FirstOrDefaultAsync(x => x.TelegramId == pid, cancellationToken);
        if (player is null)
        {
            return new JsonResult(string.Empty);
        }

        var gamePlayers = await database.GamePlayers.AsNoTracking().Where(x => x.PlayerId == player.Id).ToListAsync(cancellationToken);
        if (gamePlayers.Count == 0)
        {
            return new JsonResult(string.Empty);
        }

        var roles = await database.PlayerRoles(pid).AsNoTracking().ToListAsync(cancellationToken);
        var killed = (await database.PlayerMostKilled(pid).AsNoTracking().ToListAsync(cancellationToken)).FirstOrDefault();
        var killedBy = (await database.PlayerMostKilledBy(pid).AsNoTracking().ToListAsync(cancellationToken)).FirstOrDefault();
        var gamesPlayed = gamePlayers.Count;
        var won = gamePlayers.Count(x => x.Won);
        var lost = gamesPlayed - won;
        var survived = gamePlayers.Count(x => x.Survived);
        var mostCommonRole = roles.OrderByDescending(x => x.times).FirstOrDefault();
        var killedLink = killed is null ? "No kills?" : $"<a href='../Player/{killed.TelegramId}'>{killed.Name}</a>";
        var killedByLink = killedBy is null ? "Not killed?" : $"<a href='../Player/{killedBy.TelegramId}'>{killedBy.Name}</a>";

        if (json)
        {
            return new JsonResult(new
            {
                gamesPlayed,
                won = new { total = won, percent = won * 100 / gamesPlayed },
                lost = new { total = lost, percent = lost * 100 / gamesPlayed },
                survived = new { total = survived, percent = survived * 100 / gamesPlayed },
                mostCommonRole = mostCommonRole?.role ?? "WHAT? YOU HAVEN'T PLAYED?",
                mostKilled = killed is null ? null : new { name = killed.Name, id = killed.TelegramId, link = killedLink, times = killed.times },
                mostKilledBy = killedBy is null ? null : new { name = killedBy.Name, id = killedBy.TelegramId, link = killedByLink, times = killedBy.times }
            });
        }

        var html = $"<table class=\"table table-hover\"><tbody>" +
            $"<tr><td>Games played total</td><td><b>{gamesPlayed}</b></td></tr>" +
            $"<tr><td>Games won</td><td><b>{won}</b></td><td>{won * 100 / gamesPlayed}%</td></tr>" +
            $"<tr><td>Games lost</td><td><b>{lost}</b></td><td>{lost * 100 / gamesPlayed}%</td></tr>" +
            $"<tr><td>Games survived</td><td><b>{survived}</b></td><td>{survived * 100 / gamesPlayed}%</td></tr>" +
            $"<tr><td>Most common role</td><td><b>{mostCommonRole?.role ?? "WHAT? YOU HAVEN'T PLAYED?"}</b></td><td>{mostCommonRole?.times ?? 0} times</td></tr>" +
            $"<tr><td>Most killed</td><td><b>{killedLink}</b></td><td>{killed?.times ?? 0} times</td></tr>" +
            $"<tr><td>Most killed by</td><td><b>{killedByLink}</b></td><td>{killedBy?.times ?? 0} times</td></tr>" +
            "</tbody></table>";
        return new JsonResult(html);
    }

    [HttpGet]
    public async Task<IActionResult> PlayerKills(long pid, bool json = false, CancellationToken cancellationToken = default)
    {
        return await PlayerKillList(pid, json, false, cancellationToken);
    }

    [HttpGet]
    public async Task<IActionResult> PlayerKilledBy(long pid, bool json = false, CancellationToken cancellationToken = default)
    {
        return await PlayerKillList(pid, json, true, cancellationToken);
    }

    [HttpGet]
    public async Task<IActionResult> PlayerDeaths(long pid, bool json = false, CancellationToken cancellationToken = default)
    {
        if (!HasDatabaseConnection())
        {
            return DatabaseUnavailable();
        }

        var player = await database.Players.AsNoTracking().FirstOrDefaultAsync(x => x.TelegramId == pid, cancellationToken);
        if (player is null)
        {
            return new JsonResult(string.Empty);
        }

        var deathCounts = await database.GameKills.AsNoTracking()
            .Where(x => x.VictimId == player.Id && x.KillMethodId != 0)
            .GroupBy(x => x.KillMethodId)
            .Select(x => new { Method = x.Key, Count = x.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToListAsync(cancellationToken);
        var totalDeaths = deathCounts.Sum(x => x.Count);
        var results = deathCounts.Select(x => new
        {
            method = Enum.GetName((KillMthd)x.Method) ?? $"Unknown ({x.Method})",
            percent = totalDeaths == 0 ? "0.0" : (x.Count * 100d / totalDeaths).ToString("#0.0")
        }).ToList();

        if (json)
        {
            return new JsonResult(results);
        }

        var html = "<table class=\"table table-hover\"><tbody><tr><th>Death Type</th><th>Percentage</th></tr>" +
            (results.Count == 0
                ? "<tr><td><b>No deaths!</b></td><td></td></tr>"
                : string.Join(string.Empty, results.Select(x => $"<tr><td><b>{x.method}</b></td><td>{x.percent}%</td></tr>"))) +
            "</tbody></table>";
        return new JsonResult(html);
    }

    private async Task<IActionResult> PlayerKillList(long pid, bool json, bool killedBy, CancellationToken cancellationToken)
    {
        if (!HasDatabaseConnection())
        {
            return DatabaseUnavailable();
        }

        var player = await database.Players.AsNoTracking().FirstOrDefaultAsync(x => x.TelegramId == pid, cancellationToken);
        if (player is null)
        {
            return new JsonResult(string.Empty);
        }

        if (json)
        {
            if (killedBy)
            {
                var killedByRows = (await database.PlayerMostKilledBy(pid).AsNoTracking().ToListAsync(cancellationToken)).Take(5);
                return new JsonResult(killedByRows.Select(x => new { name = x.Name, times = x.times }));
            }

            var killedRows = (await database.PlayerMostKilled(pid).AsNoTracking().ToListAsync(cancellationToken)).Take(5);
            return new JsonResult(killedRows.Select(x => new { name = x.Name, times = x.times }));
        }

        var rows = killedBy
            ? (await database.PlayerMostKilledBy(pid).AsNoTracking().ToListAsync(cancellationToken)).Take(5).Select(x => new { x.Name, x.times })
            : (await database.PlayerMostKilled(pid).AsNoTracking().ToListAsync(cancellationToken)).Take(5).Select(x => new { x.Name, x.times });
        var html = "<table class=\"table table-hover\"><tbody><tr><th>Player Name</th><th>Times</th></tr>" +
            string.Join(string.Empty, rows.Select(x => $"<tr><td><b>{x.Name}</b></td><td>{x.times}</td></tr>")) +
            "</tbody></table>";
        return new JsonResult(html);
    }

    [HttpGet]
    public async Task<IActionResult> PlayerAchievements(long pid, bool json = false, CancellationToken cancellationToken = default)
    {
        return await PlayerAchievementList(pid, json, false, cancellationToken);
    }

    [HttpGet]
    public async Task<IActionResult> PlayerLockedAchievements(long pid, bool json = false, CancellationToken cancellationToken = default)
    {
        return await PlayerAchievementList(pid, json, true, cancellationToken);
    }

    private async Task<IActionResult> PlayerAchievementList(long pid, bool json, bool locked, CancellationToken cancellationToken)
    {
        if (!HasDatabaseConnection())
        {
            return DatabaseUnavailable();
        }

        var player = await database.Players.AsNoTracking().FirstOrDefaultAsync(x => x.TelegramId == pid, cancellationToken);
        if (player is null)
        {
            return new JsonResult(string.Empty);
        }

        var achievements = player.NewAchievements is null
            ? new BitArray(200)
            : new BitArray(player.NewAchievements);
        var unlocked = achievements.GetUniqueFlags().ToHashSet();
        var values = Enum.GetValues<AchievementsReworked>()
            .Where(x => x is not AchievementsReworked.OHAIDER and not AchievementsReworked.None)
            .Where(x => locked ? !unlocked.Contains(x) : unlocked.Contains(x));

        if (json)
        {
            return new JsonResult(values.Select(x => new { name = x.GetName(), description = x.GetDescription() }));
        }

        var html = "<table class=\"table table-hover\"><tbody>" +
            string.Join(string.Empty, values.Select(x => $"<tr><td><b>{x.GetName()}</b></td><td>{x.GetDescription()}</td></tr>")) +
            "</tbody></table>";
        return new JsonResult(html);
    }

    private bool HasDatabaseConnection() => !string.IsNullOrWhiteSpace(configuration.GetConnectionString("BotConnectionString"));

    private ObjectResult DatabaseUnavailable() => Problem(
        "Statistics are temporarily unavailable because the database connection is not configured.",
        statusCode: StatusCodes.Status503ServiceUnavailable);
}

internal enum KillMthd
{
    None, Lynch, Eat, Shoot, VisitWolf, VisitVictim, GuardWolf, Detected, Flee, Hunt, HunterShot, LoverDied,
    SerialKilled, HunterCult, GuardKiller, VisitKiller, Idle, Suicide, StealKiller, Chemistry, FallGrave, Spotted,
    Burn, VisitBurning
}
