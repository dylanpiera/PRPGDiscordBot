using System;
using System.Collections.Generic;
using System.Text;

namespace PRPGDiscordBot.Models
{
    [Serializable]
    public class Move
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}