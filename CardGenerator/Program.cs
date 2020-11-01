using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


class Program
{
    public const string SourcePath = "E:/Workspace/VisualStudio/CardGenerator/CardGenerator/Images/"; // Images are saved to this folder

    static void Main(string[] args)
    {
        List<Card> cards = new List<Card>();

        var gsh = new GoogleSheetsHelper("auth.json", "1YnicTqSHrWr7J6YLfHImQd5V2ygZjRPkzh3CkdpW3e4");
        var gsp = new GoogleSheetParameters() { RangeColumnStart = 1, RangeRowStart = 1, RangeColumnEnd = 7, RangeRowEnd = 1000, FirstRowIsHeaders = true, SheetName = "Cards" };
        List<ExpandoObject> rowValues = gsh.GetDataFromSheet(gsp);

        foreach(ExpandoObject row in rowValues) // row ist eine Liste von KeyValuePairs. Diese enthalten als Key den Namen der Spalte (zB "Angriff") und als Value den Wert (zB "7")
        {
            dynamic card = (dynamic)row;
            string cardName = (string)card.Name;

            if (!File.Exists(SourcePath + "Images/" + FileNameForCard(cardName)))
            {
                Console.WriteLine("ERROR: Picture not found for " + cardName);
            }
            switch (card.Type)
            {
                case "Minion":
                    int cost = card.Cost == "X" ? 0 : int.Parse(card.Cost);
                    bool costX = card.Cost == "X";
                    int attack = int.Parse(card.Attack);
                    int health = int.Parse(card.Health);
                    cards.Add(new Minion(cardName, cost, attack, health, card.Class, card.Text, costX));
                    break;

                case "Spell":
                    cost = card.Cost == "X" ? 0 : int.Parse(card.Cost);
                    costX = card.Cost == "X";
                    cards.Add(new Spell(cardName, cost, card.Text, costX));
                    break;

                default:
                    Console.WriteLine(card.Name + " has unknown type " + card.Type);
                    break;
            }
        }

        Console.WriteLine("--------------------------------------------");

        foreach(Card c in cards)
        {
            Console.WriteLine("Generating image for " + c.Name);
            CardImageGenerator.GenerateImage(c, SourcePath + "Generated/");
        }

        Console.WriteLine("--------------------------------------------");
        Console.WriteLine("Successfully generated " + cards.Count + " card images.\n\nPress any key to close the window.");

        Console.ReadKey();
    }
    
    public static string FileNameForCard(string cardName)
    {
        return cardName.ToLower().Replace(" ", "").Replace("ö", "oe").Replace("ä", "ae").Replace("ü", "ue").Replace(",", "").Replace("ñ","n") + ".png";
    }
}

