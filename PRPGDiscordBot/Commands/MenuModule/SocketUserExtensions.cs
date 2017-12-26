using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRPGDiscordBot.Commands.MenuModule
{
    public static class SocketUserExtensions
    {
        public static HashSet<SocketUser> Subscribed = new HashSet<SocketUser>();

        public static void Subscribe(this SocketUser user)
        {
            try
            {
                if (Subscribed.Contains(user))
                    return;

                Subscribed.Add(user);
            }
            catch (Exception e)
            {
                Task.Run(async () => await Program.Log(e.ToString(), "SocketUserExtensions -> Subscribe", Discord.LogSeverity.Error));
            }
        }

        public static bool IsSubscribed(this SocketUser user)
        {
            try
            {
                if (Subscribed.Contains(user))
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                Task.Run(async () => await Program.Log(e.ToString(), "SocketUserExtensions -> IsSubscribed", Discord.LogSeverity.Error));
                return false;
            }
        }

        public static void Unsubscribe(this SocketUser user)
        {
            try
            {
                if (Subscribed.Contains(user))
                    Subscribed.Remove(user);
            }
            catch (Exception e)
            {
                Task.Run(async () => await Program.Log(e.ToString(), "SocketUserExtensions -> UnSubscribe", Discord.LogSeverity.Error));
            }
        }
    }
}
