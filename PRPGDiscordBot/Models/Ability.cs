using System;
using PRPGDiscordBot.Helpers;
using PRPGDiscordBot.Commands;

namespace PRPGDiscordBot.Models
{
    [Serializable]
    public class Ability
    {
        private string name;
        public string Name { get => this.Format(name); set => name = value; }

        public string Format(string toFormat)
        {
            return toFormat.Replace("-", " ").Capatalize();
        }
    }
}