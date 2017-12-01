using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRPGDiscordBot.Models
{
    /// TODO: Implement datastructures as the following: 
    /// Trainer -> {Team, Money, Items, Badges(maybe move these in items?)}
    /// Team -> {Pkmn1, Pkmn2, etc.}
    /// Pokemon -> {NationalDexNumber, Nickname, Ability, Level, Stats, Status, Shiny, Form, Pokeball, Happiness, HeldItem, Moves}
    /// Stats -> {CurHP, MaxHP, Atk, Def, Sp.Atk, Sp.Def, Speed}
    /// Moves -> {Move 1, Move 2, Move 3, Move 4}
    class Trainer
    {
        private Team team;
        private int money;
        private Items items;

    }
}
