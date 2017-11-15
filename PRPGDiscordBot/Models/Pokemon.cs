using System;
using System.Collections.Generic;
using System.Text;
using PokeAPI;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using PRPGDiscordBot.Commands;

namespace PRPGDiscordBot.Models
{
    [Serializable]
    public class Pokemon : IPokemon
    {
        public Pokemon()
        {
            ability = new Ability();

        }

        private int id;
        private string nickname;
        private Ability ability;
        private int level;
        private Stats stats;
        private Status status;
        private bool shiny;
        private int form;
        private PokeBallType pokeBallType;
        private int happiness;
        private Item heldItem;
        private Moves moves;

        public int ID
        {
            get => id; set
            {
                if (value < 0 || value > 802)
                    throw new ArgumentOutOfRangeException("ID", value, "ID Must be a valid National Dex ID, between 0 and 802.");
                id = value;
            }
        }
        public string Nickname
        {
            get
            {
                if (!string.IsNullOrEmpty(nickname))
                    return nickname;
                else
                    return null;
            }
            set => nickname = value;
        }
        public Ability Ability { get => ability; set => ability = value; }
        public int Level
        {
            get => level; set
            {
                if (value < 1 || value > 100)
                    throw new ArgumentOutOfRangeException("Level", value, "Level Must be between 1 and 100.");
                level = value;
            }
        }
        public Stats Stats { get => stats; set => stats = value; }
        public Status Status { get => status; set => status = value; }
        public bool Shiny { get => shiny; set => shiny = value; }
        public int Form { get => form; set => form = value; }
        public PokeBallType PokeBallType { get => pokeBallType; set => pokeBallType = value; }
        public int Happiness
        {
            get => happiness; set
            {
                if (value < 0 || value > 255)
                    throw new ArgumentOutOfRangeException("Happiness", value, "Happiness value Must be between 0 and 255.");
                happiness = value;
            }
        }
        public Item HeldItem { get => heldItem; set => heldItem = value; }
        public Moves Moves { get => moves; set => moves = value; }

        public string ToString(string speciesName)
        {
            string toReturn;
            toReturn =
                $"**Species**\n" +
                $"_{speciesName.Capatalize()}_\n\n" +
                $"**Level**\n" +
                $"{Level}\n\n" +
                $"**Ability**\n" +
                $"{Ability.Name}\n\n" +
                $"**Item**\n" +
                "None\n\n" + //TODO: $"{HeldItem.ToString()}" +
                $"**Stats**\n" + //TODO: Fix Spacing
                $"_HP_:\t{stats.CurHP}/{stats.MaxHP}\n" +
                $"_Atk_:\t{stats.Atk}\n" +
                $"_Def_:\t{stats.Def}\n" +
                $"_Sp.Atk_:\t{stats.SpAtk}\n" +
                $"_Sp.Def_:\t{stats.SpDef}\n" +
                $"_Speed_:\t{stats.Speed}\n" +
                $"_Happiness_:\t{Happiness}\n\n";
            if (Status != Status.None)
                toReturn +=
                    "**Status**\n" +
                    $"_{Status.ToString()}_\n\n";
            toReturn += "**Moves**\n";
            foreach (Move move in Moves)
            {
                toReturn += $"{move.Name}\n";
            }

            return toReturn;
        }

        public static Stats GenerateStarterStats(PokeAPI.Pokemon p)
        {
            Stats stats = new Stats
            {
                ///Calculation HP       =   (((base + IV) * 2) * level) / 100) + level + 10
                ///Calculation other    =    (((base + IV) * 2) * level) / 100) + 5

                Speed = ((((p.Stats[0].BaseValue + 31) * 2) * 5) / 100) + 5,
                SpDef = ((((p.Stats[1].BaseValue + 31) * 2) * 5) / 100) + 5,
                SpAtk = ((((p.Stats[2].BaseValue + 31) * 2) * 5) / 100) + 5,
                Def = ((((p.Stats[3].BaseValue + 31) * 2) * 5) / 100) + 5,
                Atk = ((((p.Stats[4].BaseValue + 31) * 2) * 5) / 100) + 5,
                MaxHP = ((((p.Stats[5].BaseValue + 31) * 2) * 5) / 100) + 15
            };
            stats.CurHP = stats.MaxHP;

            return stats;
        }

        public static Moves GenerateStarterMoves(PokeAPI.Pokemon p)
        {
            Moves moves = new Moves();

            foreach (var x in p.Moves)
            {
                foreach (var y in x.VersionGroupDetails)
                {
                    if (y.VersionGroup.Name == "sun-moon" && y.LearnedAt <= 5 && y.LearnMethod.Name == "level-up")
                    {
                        moves.Add(new Move() { Name = x.Move.Name });
                    }
                }
            }

            if (p.ID == 133)
                moves.Add(new Move() { Name = "Tackle" });

            return moves;
        }
    }

    public interface IPokemon
    {
        int ID { get; set; }
        string Nickname { get; set; }
        Ability Ability { get; set; }
        int Level { get; set; }
        Stats Stats { get; set; }
        Status Status { get; set; }
        bool Shiny { get; set; }
        int Form { get; set; }
        PokeBallType PokeBallType { get; set; }
        int Happiness { get; set; }
        Item HeldItem { get; set; }
        Moves Moves { get; set; }
    }

    [Serializable]
    public enum Status
    {
        None
    }

    [Serializable]
    public enum PokeBallType
    {
        PokeBall
    }
}
