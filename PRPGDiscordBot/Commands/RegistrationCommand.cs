using Discord;
using Discord.Commands;
using Discord.WebSocket;
using PokeAPI;
using PRPGDiscordBot.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRPGDiscordBot.Commands
{
    public class RegistrationCommand : ModuleBase
    {
        public static Dictionary<ulong, Func<SocketMessage, Task>> Events = new Dictionary<ulong, Func<SocketMessage, Task>>();

        public static readonly List<string> AvailableStarters = new List<string> { "Bulbasaur", "Squirtle", "Charmander", "Pikachu", "Eevee" };

        [Command("Register")]
        public async Task Register()
        {
            if (await DatabaseHelper.GetClosedConnection().IsUserRegistered(Context.User.Id))
            {
                //TODO: Tell them no.
                return;
            }

            await Context.Channel.SendMessageAsync("", false, new EmbedBuilder() { Title = "== PRPG REGISTRATION [1/4] ==", Description = $"Welcome {(Context.User as IGuildUser).Nickname ?? Context.User.Username} to the PRPG.\n\nRegistration is possible in two ways. Please select an option:\n1. Choose Starter\n2. ~~Personality Quiz~~ [**Disabled**]", Color = Color.Gold, ThumbnailUrl = Context.User.GetAvatarUrl() });

            Func<SocketMessage, Task> eventHandler = async (e) => await ChooseStarter(e, Context.User.Id);
            Events.Add(Context.User.Id, eventHandler);
            (Context.Client as DiscordSocketClient).MessageReceived += eventHandler;
        }

        private async Task ChooseStarter(SocketMessage arg, ulong uuid)
        {
            if (arg.Author.Id != uuid)
                return;
            if (int.TryParse(arg.Content, out int choice))
            {
                switch (choice)
                {
                    case 1:
                        EmbedBuilder builder = new EmbedBuilder() { Title = "== PRPG REGISTRATION [2/4] ==", Description = $"Please pick one of the following starters:\n", Color = Color.Gold, ThumbnailUrl = Context.User.GetAvatarUrl() };

                        for (int i = 0; i < AvailableStarters.Count; i++)
                        {
                            builder.Description += $"{i + 1}.\t{AvailableStarters[i]}\n";
                        }

                        (Context.Client as DiscordSocketClient).MessageReceived -= Events[uuid];
                        Events[uuid] = (e) => SelectStarter(e, uuid);
                        (Context.Client as DiscordSocketClient).MessageReceived += Events[uuid];
                        await arg.Channel.SendMessageAsync("", false, builder.Build());
                        break;
                    case 2:
                    //Remove comments and add personality quiz mode here.
                    //break;
                    default:
                        return;
                }
            }
        }

        private async Task SelectStarter(SocketMessage arg, ulong uuid)
        {
            if (arg.Author.Id != uuid)
                return;

            string msg = arg.Content;
            if (int.TryParse(msg, out int numChoice))
            {
                if (numChoice < 0 || numChoice >= AvailableStarters.Count)
                    return;

                string starterName = AvailableStarters[--numChoice];

                EmbedBuilder builder = new EmbedBuilder() { Title = "== PRPG REGISTRATION [3/4] ==", Description = $"{(Context.User as IGuildUser).Nickname ?? Context.User.Username}, You chose {starterName}.\nConfirm by typing \"I choose you!\"", Color = Color.Gold, ThumbnailUrl = $"https://img.pokemondb.net/sprites/black-white/anim/normal/{starterName.ToLower()}.gif" };

                (Context.Client as DiscordSocketClient).MessageReceived -= Events[uuid];
                Events[uuid] = (e) => ConfirmStarter(e, uuid, starterName);
                (Context.Client as DiscordSocketClient).MessageReceived += Events[uuid];

                IUserMessage BotMessage = await arg.Channel.SendMessageAsync("", false, builder.Build());
            }
        }

        private async Task ConfirmStarter(SocketMessage arg, ulong uuid, string starterName)
        {
            if (arg.Author.Id != uuid)
                return;
            if (arg.Content.ToLower() == "i choose you!" || arg.Content.ToLower() == "i choose you")
            {
                if (await DatabaseHelper.GetClosedConnection().RegisterUser(uuid, await this.CreateStarterPokemonXMLAsync(starterName)))
                {
                    EmbedBuilder builder = new EmbedBuilder() { Title = "== PRPG REGISTRATION [4/4] ==", Description = $"Congratulations, you've succesfully registered!", ThumbnailUrl = $"https://img.pokemondb.net/sprites/black-white/anim/normal/{starterName.ToLower()}.gif" };
                    await arg.Channel.SendMessageAsync("", false, builder.Build());
                }
                else
                {
                    await arg.Channel.SendMessageAsync("There appears to be an error. Please contact an administrator.");
                    (Context.Client as DiscordSocketClient).MessageReceived -= Events[uuid];
                }
            }
            else
            {
                EmbedBuilder builder = new EmbedBuilder() { Title = "== PRPG REGISTRATION [2/4] ==", Description = $"Please pick one of the following starters:\n", Color = Color.Gold, ThumbnailUrl = Context.User.GetAvatarUrl() };

                for (int i = 0; i < AvailableStarters.Count; i++)
                {
                    builder.Description += $"{i + 1}.\t{AvailableStarters[i]}\n";
                }
                (Context.Client as DiscordSocketClient).MessageReceived -= Events[uuid];
                Events[uuid] = (e) => SelectStarter(e, uuid);
                (Context.Client as DiscordSocketClient).MessageReceived += Events[uuid];
                await arg.Channel.SendMessageAsync("", false, builder.Build());
            }
        }

        private async Task<string> CreateStarterPokemonXMLAsync(string starterName)
        {
            PokeAPI.Pokemon p = await DataFetcher.GetNamedApiObject<PokeAPI.Pokemon>(starterName.ToLower());

            Models.Pokemon pokemon = new Models.Pokemon() {ID = p.ID, Level = 5, PokeBallType = Models.PokeBallType.PokeBall, Form = 0, Happiness = 0, Nickname = "", Shiny = false, Status = Models.Status.None};
            pokemon.Stats = Models.Pokemon.GenerateStarterStats(p);
            pokemon.Moves = Models.Pokemon.GenerateStarterMoves(p);
            pokemon.Ability.Name = p.Abilities[0].Ability.Name;

            return pokemon.Serialize();

        }
    }
}
