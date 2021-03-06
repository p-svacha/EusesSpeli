﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PlayableCard : Card
{
    public string Acronym;
    public int Cost;
    public bool CostX; // If true, the player can choose the card cost

    public PlayableCard(string name, int cost, string text, bool costX = false) :base(name, text)
    {
        Cost = cost;
        CostX = costX;
        if (costX) Cost = 0;
    }
}

