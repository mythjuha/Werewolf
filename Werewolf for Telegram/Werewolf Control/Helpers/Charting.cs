using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using ScottPlot;
using Plot = ScottPlot.Plot;
using Telegram.Bot;
using Telegram.Bot.Types;
using Werewolf_Control.Handler;
using Werewolf_Control.Models;
using Color = System.Drawing.Color;

namespace Werewolf_Control.Helpers
{
    public static class Charting
    {
        public static void TeamWinChart(string input, Update u)
        {
            //first we need to get the start date / timespan, otherwise default.
            var start = new DateTime(2016, 5, 15);
            var mode = "";
            if (!String.IsNullOrWhiteSpace(input))
            {
                var args = input.Split(' ');
                int amount = 0;
                if (int.TryParse(args[0], out amount) && args.Length >= 2)
                {
                    //get the interval
                    switch (args[1])
                    {
                        case "weeks":
                        case "week":
                            start = DateTime.UtcNow.AddDays(-(amount * 7));
                            break;
                        case "day":
                        case "days":
                            start = DateTime.UtcNow.AddDays(-amount);
                            break;
                        case "hour":
                        case "hours":
                            start = DateTime.UtcNow.AddHours(-amount);
                            break;
                        default:
                            Bot.Api.SendTextMessageAsync(chatId: u.Message.Chat.Id, text: "Acceptable intervals are: hour(s), day(s), week(s)", messageThreadId: u.Message.MessageThreadId);
                            break;
                    }
                }
                
                if (args.Length == 3)
                    mode = $"AND gm.MODE = '{args[2]}'";

                if (args.Length == 1)
                    mode = $"AND gm.MODE = '{args[0]}'";
            }
            
            var query = $@"SELECT x.Players,
 Round((COUNT (x.GameId) * 100.0 / sum (count(x.GameId)) OVER (PARTITION BY Players)), 2) AS Wins,
sum(count(x.Gameid)) over (partition by players) as Games
 , X.Winner AS Team
 FROM
 (SELECT count (gp.PlayerId) AS Players, gp.GameId, CASE WHEN gm.Winner = 'Wolves' THEN 'Wolf' ELSE gm.Winner END AS Winner
 FROM Game AS gm
 INNER JOIN GamePlayer AS gp ON gp.GameId = gm.Id
 WHERE gm.Winner is not null AND gm.TimeStarted > '{ start.ToString("yyyy-MM-dd HH:mm:ss")}' {mode}
 GROUP BY gp.GameId, gm.Winner
 HAVING COUNT (gp.PlayerId)> = 5)
 AS x 
 GROUP BY x.Winner, x.Players
 ORDER BY x.Players, Wins DESC";

            var result = new List<TeamWinResult>();
            using (var db = new WWCoreContext())
            {
                result = db.ExecuteQuery<TeamWinResult>(query);
            }

            // ScottPlot is used headlessly because the chart is written to disk for Telegram delivery.
            var chart = new Plot(1000, 400);
            chart.Title("Team win percentage by player count");
            chart.XLabel("Players");
            chart.YLabel("Wins (%)");

            foreach (var team in new[] { "Wolf", "Village", "Tanner", "Cult", "SerialKiller", "Arsonist", "Lovers" })
            {
                Color color;
                switch (team)
                {
                    case "Wolf":
                        color = Color.SaddleBrown;
                        break;
                    case "Village":
                        color = Color.Green;
                        break;
                    case "Tanner":
                        color = Color.Red;
                        break;
                    case "Cult":
                        color = Color.Blue;
                        break;
                    case "SerialKiller":
                        color = Color.Black;
                        break;
                    case "Arsonist":
                        color = Color.Orange;
                        break;
                    case "Lovers":
                        color = Color.Pink;
                        break;
                    default:
                        color = Color.Gray;
                        break;
                }
                var teamResults = result.Where(x => x.Team == team).OrderBy(x => x.Players).ToList();
                var players = teamResults.Select(x => (double)x.Players).ToArray();
                var wins = teamResults.Select(x => (double)x.Wins).ToArray();
                if (players.Length > 0)
                    chart.AddScatter(players, wins, color, label: team, lineWidth: 2, markerSize: 5);
            }
            chart.Legend(location: Alignment.UpperRight);
            var path = Path.Combine(Bot.RootDirectory, "myChart.png");
            chart.SaveFig(path);
            SendImage(path, u.Message.Chat.Id);
            UpdateHandler.Send(result.Select(x => new { x.Players, x.Games}).Distinct().Aggregate("", (a, b) => $"{a}\n{b.Players}: {b.Games}"), u.Message.Chat.Id);
        }

        private static void SendImage(string path, long id)
        {
            var fs = new FileStream(path, FileMode.Open);
            //Bot.Api.SendPhotoAsync(id, new FileToSend("chart.png", fs));
        }
    }

    class TeamWinResult
    {

        public int Players { get; set; }
        public Decimal Wins { get; set; }
        public int Games { get; set; }
        public string Team { get; set; }
        
    }
}

