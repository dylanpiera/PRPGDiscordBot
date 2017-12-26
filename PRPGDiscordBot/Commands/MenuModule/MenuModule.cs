/*
 * 
 * THIS CODE IS DEPRECATED
 * This code is deprecated and won't be used in the future.
 * For now the code shall remain in case it can still be used, but will be deleted with the next full release.
 * 
 */

//using Discord;
//using Discord.Commands;
//using Discord.WebSocket;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PRPGDiscordBot.Commands.Menu
//{
//    /// <summary>
//    /// Big black box. For usage, see contribution guide.
//    /// </summary>
//    public class MenuModule : ModuleBase
//    {
//        public struct MenuStruct
//        {
//            public EmbedBuilder Builder;
//            public giveOptions Next;

//            public MenuStruct(EmbedBuilder builder = null, giveOptions next = null)
//            {
//                Builder = builder;
//                Next = next;
//            }
//        }

//        //A dictionary of UserID to MessageReceivedHandler.
//        public static Dictionary<ulong, Func<SocketMessage, Task>> Events = new Dictionary<ulong, Func<SocketMessage, Task>>();

//        public delegate Task giveOptions(SocketMessage socket, DiscordSocketClient client);

//        /// <summary>
//        /// Debug test version of the menu system.
//        /// </summary>
//        [Command("TestOptions")]
//        public async Task TestOptions()
//        {
//            if (Events.ContainsKey(Context.User.Id))
//            {
//                (Context.Client as DiscordSocketClient).MessageReceived -= Events[Context.User.Id];
//                Events.Remove(Context.User.Id);
//                return;
//            }
            
//            giveOptions options = OptionGenerator(new Dictionary<Func<string, bool>, MenuStruct>()
//            {
//                {ContentValidizer("yes") , new MenuStruct(new EmbedBuilder() {Description = "You answered yes."},
//                    OptionGenerator(ContentValidizer("no"), IsSameUserAs(Context.User.Id), new EmbedBuilder() {Description = "You had a change of mind, I see?" }))},
//                {ContentValidizer("no") , new MenuStruct(new EmbedBuilder() {Description = "You answered no." }, 
//                OptionGenerator(ContentValidizer("yes"), IsSameUserAs(Context.User.Id), new EmbedBuilder() {Description = "You had a change of mind, I see?" }))}
//            }, IsSameUserAs(Context.User.Id), false);   

//            Func<SocketMessage, Task> eventHandler = async (s) => await options(s, Context.Client as DiscordSocketClient);
//            Events.Add(Context.User.Id, eventHandler);
//            (Context.Client as DiscordSocketClient).MessageReceived += Events[Context.User.Id];
//        }

//        /// <summary>
//        /// Returns a ContentValidizer that checks if the input is equal to the <paramref name="validizerString"/>
//        /// </summary>
//        public static Func<string, bool> ContentValidizer(string validizerString)
//        {
//            return (s) => s == validizerString;
//        }

//        /// <summary>
//        /// Returns a UserValidizer that checks if the user ID is the same as the <paramref name="user"/>
//        /// </summary>
//        public static Func<ulong, bool> IsSameUserAs(ulong user)
//        {
//            return (u) => u == user;
//        }

//        /// <summary>
//        /// Creates a giveOptions
//        /// </summary>
//        /// <param name="contentValidizer"><see cref="ContentValidizer(string)"/></param>
//        /// <param name="userValidizer"><see cref="IsSameUserAs(ulong)"/></param>
//        /// <param name="builder">The message that should be send upon succesfully going through both validizers.</param>
//        /// <param name="endOfChain">is this the last question in this menu queue?</param>
//        /// <param name="next">the next giveOptions in the tree.</param>
//        /// <returns></returns>
//        public static giveOptions OptionGenerator(Func<string, bool> contentValidizer, Func<ulong, bool> userValidizer, EmbedBuilder builder = null,  bool endOfChain = true, giveOptions next = null)
//        {
//            return async (s, c) =>
//            {
//                if (!userValidizer(s.Author.Id))
//                    return;
//                if (contentValidizer(s.Content))
//                {
//                    if (builder == null)
//                        builder = new EmbedBuilder() { Description = "Error. No message found." };
//                    await s.Channel.SendMessageAsync("", false, builder.Build());

//                    if(endOfChain)
//                    {
//                        c.MessageReceived -= Events[s.Author.Id];
//                        Events.Remove(s.Author.Id);
//                    }
//                    else
//                    {
//                        c.MessageReceived -= Events[s.Author.Id];
//                        Events[s.Author.Id] = async (sm) => await next(sm, c);
//                        c.MessageReceived += Events[s.Author.Id];
//                    }
//                }
//            };
//        }

//        /// <summary>
//        /// Creates a list of options. WARNING: Not fully implemented yet.
//        /// </summary>
//        /// <param name="dictionary">A dictionary of MessageEventHandlers to MenuStructs. See also: <seealso cref="MenuStruct"/></param>
//        /// <param name="userValidizer"><see cref="IsSameUserAs(ulong)"/></param>
//        /// <param name="endOfChain">not implemented yet.</param>
//        public static giveOptions OptionGenerator(Dictionary<Func<string,bool>, MenuStruct> dictionary, Func<ulong, bool> userValidizer, bool endOfChain = true)
//        {
//            return async (s, c) =>
//            {
//                if (!userValidizer(s.Author.Id))
//                    return;

//                foreach (Func<string,bool> item in dictionary.Keys)
//                {
//                    if(item(s.Content))
//                    {
//                        EmbedBuilder builder = dictionary[item].Builder;
//                        giveOptions next = dictionary[item].Next;

//                        if(builder == null)
//                            builder = new EmbedBuilder() { Description = "Error. No message found." };
//                        await s.Channel.SendMessageAsync("", false, builder.Build());

//                        if (endOfChain)
//                        {
//                            c.MessageReceived -= Events[s.Author.Id];
//                            Events.Remove(s.Author.Id);
//                        }
//                        else
//                        {
//                            c.MessageReceived -= Events[s.Author.Id];
//                            Events[s.Author.Id] = async (sm) => await next(sm, c);
//                            c.MessageReceived += Events[s.Author.Id];
//                        }
//                        return;
//                    }
//                }
//            };
//        }
//    }
//}
