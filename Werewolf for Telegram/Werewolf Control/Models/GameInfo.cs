using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Telegram.Bot.Types;
using Werewolf_Control.Helpers;

namespace Werewolf_Control.Models
{
    public class GameInfo
    {
        public HashSet<long> Users { get; set; } = new HashSet<long>();  //update this to users alive
        public long GroupId { get; set; }
        public string Guid { get; set; }
        public string Language { get; set; }
        public string ChatGroup { get; set; }
        public GameState State { get; set; }
        public string NodeId { get; set; }
        public IEnumerable<dynamic> Players { get; set; }
        public int PlayerCount { get; set; }
        public GameTime Cycle { get; set; }

        public enum GameTime
        {
            Day,
            Lynch,
            Night
        }

        public void AddPlayer(Update update, string gameid)
        {
            var n = Bot.Nodes.FirstOrDefault(x => x.ClientId == NodeId);
            if (n == null) return;
            //var g = n.Games.FirstOrDefault(x => x.GroupId == update.Message.Chat.Id);
            //g?.
            Users.Add(update.Message.From.Id);
            var json = JsonSerializer.Serialize(new PlayerJoinInfo { User = update.Message.From, GameId = gameid });
            n.Broadcast(json);
        }

        public void ForceStart()
        {
            var json = JsonSerializer.Serialize(new ForceStartInfo { GroupId = GroupId });
            var n = Bot.Nodes.FirstOrDefault(x => x.ClientId == NodeId);
            n?.Broadcast(json);
        }

        public void ShowPlayers()
        {
            var json = JsonSerializer.Serialize(new PlayerListRequestInfo { GroupId = GroupId });
            var n = Bot.Nodes.FirstOrDefault(x => x.ClientId == NodeId);
            n?.Broadcast(json);
        }

        public void RemovePlayer(Update update)
        {
            var n = Bot.Nodes.FirstOrDefault(x => x.ClientId == NodeId);
            if (n == null) return;

            Users.Remove(update.Message.From.Id);
            var json = JsonSerializer.Serialize(new PlayerFleeInfo { User = update.Message.From, GroupId = update.Message.Chat.Id });
            n.Broadcast(json);
        }

        public void LoadLanguage(string fileName)
        {
            var json = JsonSerializer.Serialize(new LoadLangInfo { GroupId = GroupId, FileName = fileName });
            var n = Bot.Nodes.FirstOrDefault(x => x.ClientId == NodeId);
            n?.Broadcast(json);
        }

        public void SmitePlayer(long id)
        {
            var n = Bot.Nodes.FirstOrDefault(x => x.ClientId == NodeId);
            if (n == null) return;
            Users.Remove(id);
            var json = JsonSerializer.Serialize(new PlayerSmiteInfo { GroupId = GroupId, UserId = id });
            n.Broadcast(json);
        }

        public void SkipVote()
        {
            var n = Bot.Nodes.FirstOrDefault(x => x.ClientId == NodeId);
            var json = JsonSerializer.Serialize(new SkipVoteInfo { GroupId = GroupId });
            n?.Broadcast(json);
        }

        public void Kill()
        {
            var n = Bot.Nodes.FirstOrDefault(x => x.ClientId == NodeId);
            var json = JsonSerializer.Serialize(new GameKillInfo() { GroupId = GroupId });
            n?.Broadcast(json);
        }

        public void ExtendTime(long id, bool admin, int seconds)
        {
            var n = Bot.Nodes.FirstOrDefault(x => x.ClientId == NodeId);
            if (n == null) return;
            var json = JsonSerializer.Serialize(new ExtendTimeInfo() { GroupId = GroupId , Admin = admin, User = id, Seconds = seconds});
            n?.Broadcast(json);
        }

        public void ShowJoinButton()
        {
            var json = JsonSerializer.Serialize(new JoinButtonRequestInfo { GroupId = GroupId });
            var n = Bot.Nodes.FirstOrDefault(x => x.ClientId == NodeId);
            n?.Broadcast(json);
        }
    }

    public enum GameState
    {
        Joining, Running, Dead
    }
}
