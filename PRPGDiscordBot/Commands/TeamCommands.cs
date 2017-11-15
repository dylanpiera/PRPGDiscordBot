using Discord;
using Discord.Commands;
using MySql.Data.MySqlClient;
using PokeAPI;
using PRPGDiscordBot.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PRPGDiscordBot.Commands
{
    public class TeamCommands : ModuleBase
    {
        public static bool Private = false;

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
