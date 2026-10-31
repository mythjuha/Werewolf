using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;

namespace Werewolf_Control.Helpers
{
    internal static class UpdateHelper
    {
        private static readonly IMemoryCache AdminCache = new MemoryCache(new MemoryCacheOptions());

        internal static long[] Devs =
        {
            129046388,  //Para
            133748469,  //reny
            142032675,  //Para 2
            295152997,  //Ludwig
            106665913,  //Jeff
            1430807001, //reny2
        };

        internal static long[] LangAdmins =
        {
            267376056,  //Florian
            654995039,  //Cordarion
        };

        internal static bool IsGroupAdmin(Update update)
        {
            return IsGroupAdmin(update.Message.From.Id, update.Message.Chat.Id);
        }

        internal static bool IsGlobalAdmin(long id)
        {
            using (var db = new Database.WWCoreContext())
            {
                return db.Admins.Any(x => x.UserId == id);
            }
        }

        internal static bool IsLangAdmin(long id)
        {
            return LangAdmins.Contains(id);
        }

        internal static bool IsGroupAdmin(long user, long group)
        {
            string itemIndex = $"{group}";
            if (!AdminCache.TryGetValue(itemIndex, out List<long> admins))
            {
                //fire off admin request
                try
                {
                    //check all admins
                    var t = Bot.Api.GetChatAdministratorsAsync(chatId: group).Result;
                    admins = t.Where(x => !string.IsNullOrEmpty(x.User.FirstName)).Select(x => x.User.Id).ToList(); // if their first name is empty, the account is deleted
                    AdminCache.Set(itemIndex, admins, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                    }); // Write admin list into cache
                }
                catch
                {
                    return false;
                }

            }
            return admins.Any(x => x == user);

        }
    }
}

