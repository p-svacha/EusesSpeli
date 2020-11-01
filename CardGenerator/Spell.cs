using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Spell : PlayableCard
{
    public Spell(string name, int cost, string text, bool costX = false) : base(name, cost, text, costX)
    {
        Type = CardType.Spell;
        Acronym = "S";
    }
}

