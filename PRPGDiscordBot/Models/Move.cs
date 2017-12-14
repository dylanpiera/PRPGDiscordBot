using PRPGDiscordBot.Commands;
using System;

namespace PRPGDiscordBot.Models
{
    [Serializable]
    public class Move
    {
        private string name;
        public string Name { get => Format(name); set => name = value; }

        public string Format(string toFormat)
        {
            return toFormat.Replace("-", " ").Capatalize();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}