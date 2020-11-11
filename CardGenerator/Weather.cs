using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Weather : Card
{
    public Weather(string name, string text) : base(name, text)
    {
        Type = CardType.Weather;
    }
}

