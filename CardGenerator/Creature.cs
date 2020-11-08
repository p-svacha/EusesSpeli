using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Creature : PlayableCard
{
    public int Attack;
    public int Health;
    public string Class;

    public Creature(string name, int cost, int attack, int health, string minionClass, string text, bool costX = false) : base(name, cost, text, costX)
    {
        Type = CardType.Creature;
        Acronym = "K";

        Attack = attack;
        Health = health;
        Class = minionClass;
    }
}
