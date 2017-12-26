using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using MySql.Data.MySqlClient;
using PokeAPI;
using PRPGDiscordBot.Commands.MenuModule;
using PRPGDiscordBot.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRPGDiscordBot.Commands
{
    //TODO: Ongoing Testing - To be commented later, once fully implemented.
    public class TeamCommands : InteractiveBase
    {
        public static bool Private = false;

        [Command("team", RunMode = RunMode.Async)]
        public async Task Team()
        {
            MySqlConnection conn = DatabaseHelper.GetClosedConnection();

            if (!await conn.IsUserRegistered(Context.User.Id))
                return;

            await Context.Channel.TriggerTypingAsync();

            string p = await GetFirstPokemonString();
            Program.Log(p);

            List<string> pages = new List<string>() { "\n" + p, "Pokemon 2", "Pokemon 3", "Pokemon 4", "Pokemon 5", "Pokemon 6" };

            PaginatedMessage paginatedMessage = new PaginatedMessage()
            {
                Pages = pages,
                Color = Color.LightGrey,
                Options = new PaginatedAppearanceOptions() { DisplayInformationIcon = false, JumpDisplayOptions = JumpDisplayOptions.Never},
                Title = $"{Context.User.Username}'s Team",
                Content = ""               
            };

        await PagedReplyAsync(paginatedMessage);
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

        private async Task<string> GetFirstPokemonString()
        {
            Models.Team t = (await DatabaseHelper.GetClosedConnection().GetXMLFromDatabaseAsync("Team", "Trainers", Context.User.Id)).Deserialize<Models.Team>();
            Models.Pokemon p = t.First();
            return p.ToString((await DataFetcher.GetApiObject<PokeAPI.Pokemon>(p.ID)).Name);
        }

        [Command("pokemon")]
        public async Task Pokemon()
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
