using System;
using System.Collections.Generic;
using System.Text;

namespace PRPGDiscordBot.Models
{
    [Serializable]
    public class Pokemon : IPokemon
    {
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
        public string Nickname { get => nickname; set => nickname = value; }
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
    }

    [Serializable]
    public enum PokeBallType
    {
    }
}
