using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRPGDiscordBot.Commands.MenuModule
{
    public class MenuModule : ModuleBase
    {
        public static Dictionary<ulong, Func<SocketMessage, Task>> Events = new Dictionary<ulong, Func<SocketMessage, Task>>();

        public delegate Task giveOptions(SocketMessage socket, DiscordSocketClient client);

        [Command("TestOptions")]
        public async Task TestOptions()
        {
            if (Events.ContainsKey(Context.User.Id))
            {
                (Context.Client as DiscordSocketClient).MessageReceived -= Events[Context.User.Id];
                Events.Remove(Context.User.Id);
                return;
            }

            giveOptions options = OptionGenerator(i => i == "test", u => u == Context.User.Id);   

            Func<SocketMessage, Task> eventHandler = async (s) => await options(s, Context.Client as DiscordSocketClient);
            Events.Add(Context.User.Id, eventHandler);
            (Context.Client as DiscordSocketClient).MessageReceived += Events[Context.User.Id];
        }

        public static giveOptions OptionGenerator(Func<string, bool> contentValidizer, Func<ulong, bool> userValidizer, EmbedBuilder builder = null,  bool endOfChain = true, giveOptions next = null)
        {
            return async (s, c) =>
            {
                if (userValidizer(s.Author.Id))
                    return;
                if (contentValidizer(s.Content))
                {
                    if (builder == null)
                        builder = new EmbedBuilder() { Description = "Error. No message found." };
                    await s.Channel.SendMessageAsync("", false, builder.Build());

                    if(endOfChain)
                    {
                        c.MessageReceived -= Events[s.Author.Id];
                        Events.Remove(s.Author.Id);
                    }
                    else
                    {
                        c.MessageReceived -= Events[s.Author.Id];
                        Events[s.Author.Id] = async (sm) => await next(sm, c);
                        c.MessageReceived += Events[s.Author.Id];
                    }
                }
            };
        }
    }
}
