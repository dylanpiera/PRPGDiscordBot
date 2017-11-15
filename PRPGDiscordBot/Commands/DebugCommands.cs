using Discord;
using Discord.Commands;
using PokeAPI;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using PRPGDiscordBot.Helpers;
using System.Diagnostics;

namespace PRPGDiscordBot.Commands
{
    [Serializable]
    public struct Pokemon
    {
        public string Name;
        public string Url;
    }


    [Group("debug")]
    public class DebugCommands : ModuleBase
    {
        [Command("ping")]
        public async Task Ping()
        {
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            IUserMessage msg = await Context.Channel.SendMessageAsync("Pong!");
            await msg.ModifyAsync(x => x.Content = $"Pong! `ms{sw.ElapsedMilliseconds}`");
            sw.Stop();
        }

        [Command("nickname")]
        public async Task NicknameValidiser([Remainder] string atr)
        {
            try
            {
                await Context.Channel.SendMessageAsync("Your string is valid: " + (string)new Nickname(atr));
            }
            catch (Exception e)
            {
                await Context.Channel.SendMessageAsync("Invalid string. " + e.ToString());
            }
        }

        [Command("getTest")]
        public async Task GetTest()
        {
            IUserMessage msg = await Context.Channel.SendMessageAsync("...");

            string connStr = $"Server={Sneaky.DatabaseUrl};Uid={Sneaky.User};Database=PRPG;port=3306;Password={Sneaky.Password}";
            MySqlConnection conn = new MySqlConnection(connStr);


            Pokemon vector = (await conn.GetXMLFromDatabaseAsync("testdata", "players", Context.User.Id)).Deserialize<Pokemon>();

            //await msg.ModifyAsync(x => x.Content = $"The vector stored in the database is:\n{vector.ToString()}");
            await this.Purge(vector.Name);

        }

        [Command("setTest")]
        public async Task SetTest(string name)
        {
            IUserMessage msg = await Context.Channel.SendMessageAsync("...");

            MySqlConnection conn = DatabaseHelper.GetClosedConnection();

            try
            {
                await msg.ModifyAsync(x => x.Content = "Connecting to Database...");
                await conn.OpenAsync();
                await msg.ModifyAsync(x => x.Content = "Succesfully established connection :white_check_mark:");

                Pokemon pokemon = new Pokemon() { Name = name };



                string str = pokemon.Serialize();

                string sql = "UPDATE players SET testdata = @pokemon WHERE UserID = '" + Context.User.Id.ToString() + "'";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.Add("@pokemon", MySqlDbType.Text).Value = str;

                await Program.Log((await cmd.ExecuteNonQueryAsync()).ToString());
                await msg.ModifyAsync(x => x.Content = "Successfully updated database.");
            }
            catch (Exception e)
            {
                await Program.Log(new LogMessage(LogSeverity.Error, "Database", "error while connection to database: " + e.ToString()));
                await msg.ModifyAsync(x => x.Content = "Failed to establish connection to database. See console for error details. :x:");
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        [Command("get"), Summary("Returns with Pong!")]
        public async Task Purge(int ID)
        {
            IUserMessage msg = await Context.Channel.SendMessageAsync("Fetching Data...");

            try
            {
                PokeAPI.Pokemon p = await DataFetcher.GetApiObject<PokeAPI.Pokemon>(ID);

                EmbedBuilder builder = new EmbedBuilder() { Title = "Pokemon Request by ID", Color = Color.DarkGreen, ThumbnailUrl = p.Sprites.FrontMale };

                builder.Description = $"{FormatHelper.Capatalize(p.Name)} the {FormatHelper.Capatalize((await DataFetcher.GetApiObject<PokemonSpecies>(ID)).Genera[2].Name)}";

                EmbedFieldBuilder field = new EmbedFieldBuilder()
                {
                    Name = "Base Stats",
                    IsInline = true
                };

                foreach (PokemonStats stat in p.Stats)
                {
                    field.Value += $"{FormatHelper.Capatalize(stat.Stat.Name)}: {stat.BaseValue.ToString()}\n";
                }

                builder.AddField(field);

                await msg.ModifyAsync(x => { x.Content = ""; x.Embed = builder.Build(); });
            }
            catch
            {
                await msg.ModifyAsync(x => x.Content = "Could find pokemon with ID: " + ID);
            }
        }

        [Command("get"), Summary("Returns with Pong!"), Priority(-1)]
        public async Task Purge([Remainder()]string name)
        {
            IUserMessage msg = await Context.Channel.SendMessageAsync("Fetching Data...");

            try
            {
                PokeAPI.Pokemon p = await DataFetcher.GetNamedApiObject<PokeAPI.Pokemon>(name.ToLower());

                EmbedBuilder builder = new EmbedBuilder() { Title = "Pokemon Request by Name", Color = Color.DarkGreen, ThumbnailUrl = p.Sprites.FrontMale };

                builder.Description = $"{FormatHelper.Capatalize(p.Name)} the {FormatHelper.Capatalize((await DataFetcher.GetNamedApiObject<PokemonSpecies>(name)).Genera[2].Name)}";

                EmbedFieldBuilder field = new EmbedFieldBuilder()
                {
                    Name = "Base Stats",
                    IsInline = true
                };

                foreach (PokemonStats stat in p.Stats)
                {
                    field.Value += $"{FormatHelper.Capatalize(stat.Stat.Name)}: {stat.BaseValue.ToString()}\n";
                }

                builder.AddField(field);

                await msg.ModifyAsync(x => { x.Content = ""; x.Embed = builder.Build(); });
            }
            catch
            {
                await msg.ModifyAsync(x => x.Content = "Could find pokemon with name: " + name);
            }
        }
    }


    internal static class FormatHelper
    {
        public static string Capatalize(this string input)
        {
            return input.ToCharArray()[0].ToString().ToUpper() + input.Substring(1);
        }
    }
}