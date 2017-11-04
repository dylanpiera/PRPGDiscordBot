using System;
using System.Collections.Generic;
using System.Text;

namespace PRPGDiscordBot.Models
{
    [Serializable]
    public class Stats
    {
        private int speed;
        private int curHP;
        private int maxHP;
        private int atk;
        private int def;
        private int spAtk;
        private int spDef;

        public int CurHP { get => curHP; set => curHP = value; }
        public int MaxHP { get => maxHP; set => maxHP = value; }
        public int Atk { get => atk; set => atk = value; }
        public int Def { get => def; set => def = value; }
        public int SpAtk { get => spAtk; set => spAtk = value; }
        public int SpDef { get => spDef; set => spDef = value; }
        public int Speed { get => speed; set => speed = value; }
    }
}