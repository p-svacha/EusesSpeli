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
    public const string SourcePath = "E:/Workspace/VisualStudio/EusesSpeli/CardGenerator/Images/";
    public const bool CreateCardsWithoutImages = false;

    static void Main(string[] args)
    {
        var gsh = new GoogleSheetsHelper("auth.json", "1YnicTqSHrWr7J6YLfHImQd5V2ygZjRPkzh3CkdpW3e4");

        GeneratePlayableCards(gsh);
        GenerateWeatherCards(gsh);

        Console.WriteLine("Press any key to close the window.");
        Console.ReadKey();
    }

    private static void GeneratePlayableCards(GoogleSheetsHelper gsh)
    {
        Console.WriteLine("-------------------- Generating Playable Cards --------------------");

        List<Card> cards = new List<Card>();
        var gsp = new GoogleSheetParameters() { RangeColumnStart = 1, RangeRowStart = 1, RangeColumnEnd = 7, RangeRowEnd = 1000, FirstRowIsHeaders = true, SheetName = "Cards" };
        List<ExpandoObject> rowValues = gsh.GetDataFromSheet(gsp);

        foreach (ExpandoObject row in rowValues) // row ist eine Liste von KeyValuePairs. Diese enthalten als Key den Namen der Spalte (zB "Angriff") und als Value den Wert (zB "7")
        {
            dynamic card = (dynamic)row;
            string cardName = (string)card.Name;
            string cardText = (string)card.Text;
            bool hasPicture = File.Exists(SourcePath + "Images/" + FileNameForCard(cardName));

            if (!hasPicture)
                Console.WriteLine("ERROR: Picture not found for " + cardName + ".");

            if (CreateCardsWithoutImages || hasPicture)
                switch (card.Type)
                {
                    case "Kreatur":
                        int cost = card.Cost == "X" ? 0 : int.Parse(card.Cost);
                        bool costX = card.Cost == "X";
                        int attack = int.Parse(card.Attack);
                        int health = int.Parse(card.Health);
                        cards.Add(new Creature(cardName, cost, attack, health, card.Class, cardText, costX));
                        break;

                    case "Zauber":
                        cost = card.Cost == "X" ? 0 : int.Parse(card.Cost);
                        costX = card.Cost == "X";
                        cards.Add(new Spell(cardName, cost, cardText, costX));
                        break;

                    default:
                        Console.WriteLine("ERROR: " + card.Name + " has unknown type " + card.Type);
                        break;
                }
        }

        foreach (Card c in cards)
        {
            Console.WriteLine("Generating image for " + c.Name);
            CardImageGenerator.GenerateImage(c, SourcePath + "Generated/");
        }

        Console.WriteLine("\n\n");

        Console.WriteLine("Successfully generated " + cards.Count + " playable cards.\n\n");
    }

    private static void GenerateWeatherCards(GoogleSheetsHelper gsh)
    {
        Console.WriteLine("-------------------- Generating Weather Cards --------------------");

        List<Weather> cards = new List<Weather>();
        var gsp = new GoogleSheetParameters() { RangeColumnStart = 1, RangeRowStart = 1, RangeColumnEnd = 2, RangeRowEnd = 1000, FirstRowIsHeaders = true, SheetName = "Weather" };
        List<ExpandoObject> rowValues = gsh.GetDataFromSheet(gsp);

        foreach (ExpandoObject row in rowValues) // row ist eine Liste von KeyValuePairs. Diese enthalten als Key den Namen der Spalte (zB "Angriff") und als Value den Wert (zB "7")
        {
            dynamic card = (dynamic)row;
            string cardName = (string)card.Name;
            string cardText = (string)card.Text;
            bool hasPicture = File.Exists(SourcePath + "Images/" + FileNameForCard(cardName));

            if (!hasPicture)
                Console.WriteLine("ERROR: Picture not found for " + cardName + ".");

            if (CreateCardsWithoutImages || hasPicture)
                cards.Add(new Weather(cardName, cardText));
        }

        foreach (Card c in cards)
        {
            Console.WriteLine("Generating image for " + c.Name);
            CardImageGenerator.GenerateImage(c, SourcePath + "Generated/");
        }

        Console.WriteLine("\n\n");

        Console.WriteLine("Successfully generated " + cards.Count + " weather cards.\n\n");
    }
    
    public static string FileNameForCard(string cardName)
    {
        return cardName.ToLower().Replace(" ", "").Replace("ö", "oe").Replace("ä", "ae").Replace("ü", "ue").Replace(",", "").Replace("ñ","n").Replace(":","").Replace("-","").Replace("(","").Replace(")","").Replace("'","").Replace("!","").Replace("&","") + ".png";
    }
}

