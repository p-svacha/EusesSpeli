using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Card
{
    public CardType Type;
    public string Name;

    public Card(string name)
    {
        Name = name;
    }
}

public enum CardType
{
    Creature,
    Spell,
    Gold,
    Weather
}