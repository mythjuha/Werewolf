using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Requests;

namespace Werewolf_Node.Helpers
{
    internal static class Helpers
    {
        internal static void KickChatMember(long chatid, int userid)
        {
            var status = Program.Bot.SendRequest(new GetChatMemberRequest { ChatId = chatid, UserId = userid }).Result.Status;

            if (status == ChatMemberStatus.Administrator) //ignore admins
                return;
            //kick
            Program.Bot.SendRequest(new BanChatMemberRequest { ChatId = chatid, UserId = userid });
            //get their status
            status = Program.Bot.SendRequest(new GetChatMemberRequest { ChatId = chatid, UserId = userid }).Result.Status;
            while (status == ChatMemberStatus.Member) //loop
            {
                //wait for database to report status is kicked.
                status = Program.Bot.SendRequest(new GetChatMemberRequest { ChatId = chatid, UserId = userid }).Result.Status;
                Thread.Sleep(100);
            }
            //status is now kicked (as it should be)

            while (status != ChatMemberStatus.Left) //unban until status is left
            {
               Program.Bot.SendRequest(new UnbanChatMemberRequest { ChatId = chatid, UserId = userid });
                Thread.Sleep(100);
                status = Program.Bot.SendRequest(new GetChatMemberRequest { ChatId = chatid, UserId = userid }).Result.Status;
            }
            //yay unbanned

        }
    }
}
