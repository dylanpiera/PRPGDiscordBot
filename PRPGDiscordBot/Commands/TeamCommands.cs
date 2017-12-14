using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MySql.Data.MySqlClient;
using PokeAPI;
using PRPGDiscordBot.Commands.Menu;
using PRPGDiscordBot.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRPGDiscordBot.Commands
{
    //TODO: Ongoing Testing - To be commented later, once fully implemented.
    public class TeamCommands : ModuleBase
    {
        public static bool Private = false;

        [Command("team")]
        public async Task TeamMenu()
        {
            MySqlConnection conn = DatabaseHelper.GetClosedConnection();

            if (!await conn.IsUserRegistered(Context.User.Id))
                return;

            IUserMessage msg;

            if (Private)
                msg = await (await Context.User.GetOrCreateDMChannelAsync()).SendMessageAsync("Loading Data...");
            else
                msg = await Context.Channel.SendMessageAsync("Loading Data...");

            Func<SocketMessage, Task> eventHandler = async s => await (MenuModule.OptionGenerator(
                    new Dictionary<Func<string, bool>, MenuModule.MenuStruct> {
                        {MenuModule.ContentValidizer("1"), new MenuModule.MenuStruct(await GetFirstPokemon())},
                        {MenuModule.ContentValidizer("2"), new MenuModule.MenuStruct() }
                    },
                    MenuModule.IsSameUserAs(Context.User.Id)
                )(s,Context.Client as DiscordSocketClient));
            MenuModule.Events.Add(Context.User.Id, eventHandler);
            (Context.Client as DiscordSocketClient).MessageReceived += MenuModule.Events[Context.User.Id];

            await msg.ModifyAsync(x => x.Embed = new EmbedBuilder()
            {
                Title = "Team Manager",
                Description = "Welcome to the team manager, what would you like to do:\n" +
                "1. view first pokemon in party\n" +
                "2. exit"
            }.Build());
        }

        private async Task<EmbedBuilder> GetFirstPokemon()
        {
            Models.Team t = (await DatabaseHelper.GetClosedConnection().GetXMLFromDatabaseAsync("Team", "Trainers", Context.User.Id)).Deserialize<Models.Team>();
            Models.Pokemon p = t.First();
            PokeAPI.Pokemon pApi = await DataFetcher.GetApiObject<PokeAPI.Pokemon>(p.ID);

            EmbedBuilder builder = new EmbedBuilder()
            {
                Title = $"{(Context.User as IGuildUser).Nickname ?? Context.User.Username}'s {p.Nickname ?? pApi.Name.Capatalize()}",
                Color = Color.Teal,
                Description = p.ToString(pApi.Name),
                ThumbnailUrl = $"https://img.pokemondb.net/sprites/black-white/anim/normal/{pApi.Name}.gif"
            };

            return builder;
        }

        [Command("pokemon")]
        public async Task Team()
        {
            MySqlConnection conn = DatabaseHelper.GetClosedConnection();

            if (!await conn.IsUserRegistered(Context.User.Id))
                return;

            IUserMessage msg;

            if (Private)
                msg = await (await Context.User.GetOrCreateDMChannelAsync()).SendMessageAsync("Loading Data...");
            else
                msg = await Context.Channel.SendMessageAsync("Loading Data...");

            Models.Team t = (await conn.GetXMLFromDatabaseAsync("Team", "Trainers", Context.User.Id)).Deserialize<Models.Team>();
            Models.Pokemon p = t.First();
            PokeAPI.Pokemon pApi = await DataFetcher.GetApiObject<PokeAPI.Pokemon>(p.ID);

            EmbedBuilder builder = new EmbedBuilder()
            {
                Title = $"{(Context.User as IGuildUser).Nickname ?? Context.User.Username}'s {p.Nickname ?? pApi.Name.Capatalize()}",
                Color = Color.Teal,
                Description = p.ToString(pApi.Name),
                ThumbnailUrl = $"https://img.pokemondb.net/sprites/black-white/anim/normal/{pApi.Name}.gif"
            };

            await msg.ModifyAsync(x => { x.Content = ""; x.Embed = builder.Build(); });

        }

    }
}
