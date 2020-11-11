using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Card
{
    public CardType Type;
    public string Name;
    public string Text;

    public Card(string name, string text)
    {
        Name = name;
        Text = text;
    }
}

public enum CardType
{
    Creature,
    Spell,
    Weather
}