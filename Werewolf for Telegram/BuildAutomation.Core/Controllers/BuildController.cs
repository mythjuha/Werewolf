using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BuildAutomation.Core.Models;
using BuildAutomation.Core.Models.Build;
using BuildAutomation.Core.Models.Release;
using BuildAutomation.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Exceptions;

namespace BuildAutomation.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<BuildController> _logger;
        private readonly ITelegramNotificationService _notifications;

        public BuildController(IConfiguration configuration, ILogger<BuildController> logger,
            ITelegramNotificationService notifications)
        {
            _configuration = configuration;
            _logger = logger;
            _notifications = notifications;
        }
        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Post([FromBody] JsonElement payload)
        {
            try
            {
                var body = JsonSerializer.Serialize(payload);
                var obj = JsonSerializer.Deserialize<ReleaseEvent>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (obj?.subscriptionId != null)
                {
                    var releaseKey = _configuration["BuildAutomation:VstsReleaseId"];
                    var buildKey = _configuration["BuildAutomation:VstsBuildId"];
                    if (obj.subscriptionId == releaseKey) //can't have random people triggering this!
                    {
                        //what was released
                        var beta = obj.resource.environment.name.Contains("Beta");
                        var node = obj.resource.environment.name.Contains("Node");
                        var control = obj.resource.environment.name.Contains("Control");
                        var website = obj.resource.environment.name.Contains("Website");


                        var msg = obj.detailedMessage.markdown;
                        InlineKeyboardMarkup menu = null;
                        if (obj.resource.environment.status == "succeeded")
                        {
                            if (website)
                            {
                                msg += "The website will now be deployed.\r\n";
                            }

                            if (node || control)
                            {
                                msg += "\nDo you want me to copy the files and update?";
                                menu = new InlineKeyboardMarkup(new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Yes",
                                        $"update|{(beta ? "beta" : "release")}{(node ? "node" : "control")}"),
                                    InlineKeyboardButton.WithCallbackData("No", "update|no")
                                });
                            }

                        }


                        await _notifications.SendAsync(msg, ParseMode.Markdown, menu);
                    }

                    if (obj.subscriptionId == buildKey)
                    {
                    var build = JsonSerializer.Deserialize<BuildEvent>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        var detail = obj.detailedMessage.markdown;
                        if (detail.Contains("\r\n+ Process 'msbuild.exe'"))
                        {
                            detail = detail.Substring(0, detail.IndexOf("\r\n+ Process 'msbuild.exe'"));
                        }
                        detail = detail.Replace("\r\n+ ", "\r\n");
                        var msg = detail + "\n";
                        var urlPre = "https://github.com/GreyWolfDev/Werewolf/commit/";
                        var ver = build.resource.sourceGetVersion;
                        var commit = ver.Substring(ver.Length - 40, 40);
                        msg +=
                            $"Built with commit [{commit}]({urlPre + commit}) as latest";
                        if (build.resource.status == "succeeded")
                            msg += "\nRelease is now being created, you will be notified when it is completed.";

                        await _notifications.SendAsync(msg, ParseMode.Markdown);
                    }
                }
                else
                {
                    //github
                    var push = JsonSerializer.Deserialize<PushEvent>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    var msg =
                        $"🔨 <a href='{push.compare}'>{push.commits.Length} new commit{(push.commits.Length > 1 ? "s" : "")} to {push.repository.name}:{push._ref}</a>\n\n";
                    msg = push.commits.Aggregate(msg,
                        (current, a) => current +
                                        $"<a href='{a.url}'>{a.id.Substring(0, 7)}</a>: {a.message} ({a.author.username})\n");




                    //string path = HttpContext.Current.Server.MapPath("~/App_Data/github.json");
                    //using (var sw = new StreamWriter(path))
                    //{
                    //    foreach (var c in push.commits)
                    //        sw.WriteLine($"Commit by: {c.committer.username}\nMessage: {c.message}\n");
                    //    sw.WriteLine(body);
                    //}

                    //check what was built
                    var beta = push._ref.Contains("beta"); //beta or master - refs/head/<branch>
                    var master = push._ref.Contains("master");
                    //now check if control was changed
                    var control =
                        push.commits.Any(
                            x => x.modified.Union(x.added).Union(x.removed).Any(c => c.Contains("Werewolf Control")));
                    var node =
                        push.commits.Any(
                            x => x.modified.Union(x.added).Union(x.removed).Any(c => c.Contains("Werewolf Node")));
                    var website =
                        push.commits.Any(
                            x => x.modified.Union(x.added).Union(x.removed).Any(c => c.Contains("Werewolf Website")));

                    if ((!beta && !master) || (!control && !node && !website)) //nothing to build
                    {
                        await _notifications.SendAsync(msg, ParseMode.Html, disableWebPagePreview: true);
                        return Ok();
                    }



                    var none = InlineKeyboardButton.WithCallbackData("No", "build|no");
                    var yes = "Yes";
                    var betaControl = InlineKeyboardButton.WithCallbackData(yes, "build|betacontrol");
                    var betaNode = InlineKeyboardButton.WithCallbackData(yes, "build|betanode");
                    var betaBoth = InlineKeyboardButton.WithCallbackData(yes, "build|betaboth");

                    var releaseControl = InlineKeyboardButton.WithCallbackData(yes, "build|releasecontrol");
                    var releaseNode = InlineKeyboardButton.WithCallbackData(yes, "build|releasenode");
                    //var releaseBoth = InlineKeyboardButton.WithCallbackData(yes, "build|releaseboth");
                    var releaseWebSite = InlineKeyboardButton.WithCallbackData(yes, "build|releasewebsite");
                    //var releaseWN = InlineKeyboardButton.WithCallbackData(yes, "build|releasenodewebsite");
                    //var releaseWC = InlineKeyboardButton.WithCallbackData(yes, "build|releasecontrolwebsite");
                    //var releaseAll = InlineKeyboardButton.WithCallbackData(yes, "build|releasewebsitebot");
                    Menu menu;

                    msg += "\nThis commit contains changes to ";
                    if (beta)
                    {
                        if (control && node && website)
                        {
                            menu = new Menu(1, new List<InlineKeyboardButton>
                            {
                                InlineKeyboardButton.WithCallbackData("All of it!", "build|betacontrolwebsitenode"),
                                InlineKeyboardButton.WithCallbackData("Control & Node", "build|betacontrolnode"),
                                InlineKeyboardButton.WithCallbackData("Website & Control", "build|betacontrolwebsite"),
                                InlineKeyboardButton.WithCallbackData("Website & Node", "build|betanodewebsite"),
                                InlineKeyboardButton.WithCallbackData("Website Only", "build|betawebsite"),
                                InlineKeyboardButton.WithCallbackData("Control Only", "build|betacontrol"),
                                InlineKeyboardButton.WithCallbackData("Node Only", "build|betanode"),
                                none
                            });
                            msg += "Control, Node, and Website";
                        }
                        else if (control && node)
                        {
                            menu = new Menu(1, new List<InlineKeyboardButton>
                            {
                                InlineKeyboardButton.WithCallbackData("Control & Node", "build|betacontrolnode"),
                                InlineKeyboardButton.WithCallbackData("Control Only", "build|betacontrol"),
                                InlineKeyboardButton.WithCallbackData("Node Only", "build|betanode"),
                                none
                            });
                            msg += "Control and Node";
                        }
                        else if (control && website)
                        {
                            menu = new Menu(1, new List<InlineKeyboardButton>
                            {
                                InlineKeyboardButton.WithCallbackData("Website & Control", "build|betacontrolwebsite"),
                                InlineKeyboardButton.WithCallbackData("Website Only", "build|betawebsite"),
                                InlineKeyboardButton.WithCallbackData("Control Only", "build|betacontrol"),
                                none
                            });
                            msg += "Control and Website";
                        }
                        else if (node && website)
                        {
                            menu = new Menu(1, new List<InlineKeyboardButton>
                            {
                                InlineKeyboardButton.WithCallbackData("Website & Node", "build|betanodewebsite"),
                                InlineKeyboardButton.WithCallbackData("Website Only", "build|betawebsite"),
                                InlineKeyboardButton.WithCallbackData("Node Only", "build|betanode"),
                                none
                            });
                            msg += "Node and Website";
                        }
                        else if (control)
                        {
                            menu = new Menu(1, new List<InlineKeyboardButton>
                            {
                                betaControl,
                                none
                            });
                            msg += "Control only";
                        }
                        else if (node)
                        {
                            menu = new Menu(1, new List<InlineKeyboardButton>
                            {
                                betaNode,
                                none
                            });
                            msg += "Node only";
                        }
                        else //if (website)
                        {
                            menu = new Menu(1, new List<InlineKeyboardButton>
                            {
                                releaseWebSite,
                                none
                            });
                            msg += "Website Only";
                        }
                    }
                    else
                    {
                        if (control && node && website)
                        {
                            menu = new Menu(1, new List<InlineKeyboardButton>
                            {
                                InlineKeyboardButton.WithCallbackData("All of it!", "build|releasecontrolwebsitenode"),
                                InlineKeyboardButton.WithCallbackData("Control & Node", "build|releasecontrolnode"),
                                InlineKeyboardButton.WithCallbackData("Website & Control", "build|releasecontrolwebsite"),
                                InlineKeyboardButton.WithCallbackData("Website & Node", "build|releasenodewebsite"),
                                InlineKeyboardButton.WithCallbackData("Website Only", "build|releasewebsite"),
                                InlineKeyboardButton.WithCallbackData("Control Only", "build|releasecontrol"),
                                InlineKeyboardButton.WithCallbackData("Node Only", "build|releasenode"),
                                none
                            });
                            msg += "Control, Node, and Website";
                        }
                        else if (control && node)
                        {
                            menu = new Menu(1, new List<InlineKeyboardButton>
                            {
                                InlineKeyboardButton.WithCallbackData("Control & Node", "build|releasecontrolnode"),
                                InlineKeyboardButton.WithCallbackData("Control Only", "build|releasecontrol"),
                                InlineKeyboardButton.WithCallbackData("Node Only", "build|releasenode"),
                                none
                            });
                            msg += "Control and Node";
                        }
                        else if (control && website)
                        {
                            menu = new Menu(1, new List<InlineKeyboardButton>
                            {
                                InlineKeyboardButton.WithCallbackData("Website & Control", "build|releasecontrolwebsite"),
                                InlineKeyboardButton.WithCallbackData("Website Only", "build|releasewebsite"),
                                InlineKeyboardButton.WithCallbackData("Control Only", "build|releasecontrol"),
                                none
                            });
                            msg += "Control and Website";
                        }
                        else if (node && website)
                        {
                            menu = new Menu(1, new List<InlineKeyboardButton>
                            {
                                InlineKeyboardButton.WithCallbackData("Website & Node", "build|releasenodewebsite"),
                                InlineKeyboardButton.WithCallbackData("Website Only", "build|releasewebsite"),
                                InlineKeyboardButton.WithCallbackData("Node Only", "build|releasenode"),
                                none
                            });
                            msg += "Node and Website";
                        }
                        else if (control)
                        {
                            menu = new Menu(1, new List<InlineKeyboardButton>
                            {
                                releaseControl,
                                none
                            });
                            msg += "Control only";
                        }
                        else if (node)
                        {
                            menu = new Menu(1, new List<InlineKeyboardButton>
                            {
                                releaseNode,
                                none
                            });
                            msg += "Node only";
                        }
                        else //if (website)
                        {
                            menu = new Menu(1, new List<InlineKeyboardButton>
                            {
                                releaseWebSite,
                                none
                            });
                            msg += "Website Only";
                        }
                    }
                    msg += $", on {(beta ? "Beta" : "Release")}\n";
                    msg += "Do you want to build?";

                    await _notifications.SendAsync(msg, ParseMode.Html, menu.CreateMarkup(), true);

                    //if (beta)
                    //{
                    //    var m = "Changes on beta branch. Do you want to build the website too?";
                    //    var websiteYes = InlineKeyboardButton.WithCallbackData("Yes", "build|website");
                    //    menu = new InlineKeyboardMarkup(new[] { websiteYes, none });
                    //    await bot.SendTextMessageAsync(GroupId, m, replyMarkup: menu, parseMode: ParseMode.Html, disableWebPagePreview: true);
                    //}

                }

                return Ok();
            }
            catch (AggregateException e)
            {
                var x = e.InnerExceptions[0];
                while (x.InnerException != null)
                    x = x.InnerException;
                await _notifications.SendAsync(x.Message + "\n" + x.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, x.Message);
            }
            catch (ApiRequestException e)
            {
                var code = e.ErrorCode;
                var x = e.InnerException;
                while (x?.InnerException != null)
                    x = x.InnerException;
                await _notifications.SendAsync(x?.Message + "\n" + x?.StackTrace);
                return StatusCode(code, x?.Message);
            }
            catch (Exception e)
            {
                //string path = HttpContext.Current.Server.MapPath("~/App_Data/error");
                while (e.InnerException != null)
                    e = e.InnerException;
                await _notifications.SendAsync(e.Message + "\n" + e.StackTrace);
                //using (var sw = new StreamWriter(path))
                //{
                //    sw.WriteLine(e.Message);
                //    sw.Flush();
                //}
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }


        }
    }

    public class Menu
    {
        /// <summary>
        /// The buttons you want in your menu
        /// </summary>
        public List<InlineKeyboardButton> Buttons { get; set; }
        /// <summary>
        /// How many columns.  Defaults to 1.
        /// </summary>
        public int Columns { get; set; }

        public Menu(int col = 1, List<InlineKeyboardButton> buttons = null)
        {
            Buttons = buttons ?? new List<InlineKeyboardButton>();
            Columns = Math.Max(col, 1);
        }

        public InlineKeyboardMarkup CreateMarkup()
        {
            var col = Columns - 1;
            //this is gonna be fun...
            var final = new List<InlineKeyboardButton[]>();
            for (var i = 0; i < Buttons.Count; i++)
            {
                var row = new List<InlineKeyboardButton>();
                do
                {
                    row.Add(Buttons[i]);
                    i++;
                    if (i == Buttons.Count) break;
                } while (i % (col + 1) != 0);
                i--;
                final.Add(row.ToArray());
                if (i == Buttons.Count) break;
            }
            return new InlineKeyboardMarkup(final.ToArray());
        }
    }
}
