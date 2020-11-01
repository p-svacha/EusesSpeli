using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Minion : PlayableCard
{
    public int Attack;
    public int Health;
    public string Class;

    public Minion(string name, int cost, int attack, int health, string minionClass, string text, bool costX = false) : base(name, cost, text, costX)
    {
        Type = CardType.Minion;
        Acronym = "M";

        Attack = attack;
        Health = health;
        Class = minionClass;
    }
}
