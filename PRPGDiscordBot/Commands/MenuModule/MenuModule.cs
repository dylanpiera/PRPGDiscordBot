using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRPGDiscordBot.Commands.Menu
{
    public class MenuModule : ModuleBase
    {
        public struct MenuStruct
        {
            public EmbedBuilder Builder;
            public giveOptions Next;

            public MenuStruct(EmbedBuilder builder = null, giveOptions next = null)
            {
                Builder = builder;
                Next = next;
            }
        }

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

            giveOptions options = OptionGenerator(new Dictionary<Func<string, bool>, MenuStruct>()
            {
                {ContentValidizer("yes") , new MenuStruct(new EmbedBuilder() {Description = "You answered yes."},
                    OptionGenerator(ContentValidizer("no"), IsSameUserAs(Context.User.Id), new EmbedBuilder() {Description = "You had a change of mind, I see?" }))},
                {ContentValidizer("no") , new MenuStruct(new EmbedBuilder() {Description = "You answered no." }, 
                OptionGenerator(ContentValidizer("yes"), IsSameUserAs(Context.User.Id), new EmbedBuilder() {Description = "You had a change of mind, I see?" }))}
            }, IsSameUserAs(Context.User.Id), false);   

            Func<SocketMessage, Task> eventHandler = async (s) => await options(s, Context.Client as DiscordSocketClient);
            Events.Add(Context.User.Id, eventHandler);
            (Context.Client as DiscordSocketClient).MessageReceived += Events[Context.User.Id];
        }

        public static Func<string, bool> ContentValidizer(string validizerString)
        {
            return (s) => s == validizerString;
        }

        public static Func<ulong, bool> IsSameUserAs(ulong user)
        {
            return (u) => u == user;
        }

        public static giveOptions OptionGenerator(Func<string, bool> contentValidizer, Func<ulong, bool> userValidizer, EmbedBuilder builder = null,  bool endOfChain = true, giveOptions next = null)
        {
            return async (s, c) =>
            {
                if (!userValidizer(s.Author.Id))
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

        public static giveOptions OptionGenerator(Dictionary<Func<string,bool>, MenuStruct> dictionary, Func<ulong, bool> userValidizer, bool endOfChain = true)
        {
            return async (s, c) =>
            {
                if (!userValidizer(s.Author.Id))
                    return;

                foreach (Func<string,bool> item in dictionary.Keys)
                {
                    if(item(s.Content))
                    {
                        EmbedBuilder builder = dictionary[item].Builder;
                        giveOptions next = dictionary[item].Next;

                        if(builder == null)
                            builder = new EmbedBuilder() { Description = "Error. No message found." };
                        await s.Channel.SendMessageAsync("", false, builder.Build());

                        if (endOfChain)
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
                        return;
                    }
                }
            };
        }
    }
}
