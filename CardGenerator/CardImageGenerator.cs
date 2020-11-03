using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Media;
using System.Windows;
using System.Drawing.Text;

public static class CardImageGenerator
{
    // from https://www.printerstudio.de/pops/faq-photo.html
    private const int CardHeight = 2244;
    private const int CardWidth = 1494;

    private const string Font = "Backslash";
    private const int TitleFontSize = 74;
    private const int TextFontSize = 64;

    private const int TitleWidth = 700;
    private const int TitleY = 175;
    private const int TitleHeight = 225;

    private const int CostX = 292;
    private const int CostY = 280;
    private const int CostFontSize = 120;

    private const int TypeX = 1203;
    private const int TypeY = 280;
    private const int TypeFontSize = 112;

    private const int AttackX = 288;
    private const int AttackY = 1950;
    private const int AttackFontSize = 128;

    private const int HealthX = 1206;
    private const int HealthY = 1930;
    private const int HealthFontSize = 128;

    private const int ClassY = 1930;
    private const int ClassWidth = 640;
    private const int ClassHeight = 90;
    private const int ClassFontSizeBig = 60;
    private const int ClassFontSizeSmall = 40;

    private const int TextBoxX = 160;
    private const int TextBoxY = 1300;
    private const int TextBoxWidth = 1180;
    private const int TextBoxHeight_Minion = 550;
    private const int TextBoxHeight_Spell = 760;

    private const int CardImageX = 183;
    private const int CardImageY = 435;
    private const int CardImageWidth = 1180;
    private const int CardImageHeight = 820;

    private static System.Drawing.Brush TextColor = System.Drawing.Brushes.Black;
    private static System.Drawing.Brush CostColor = System.Drawing.Brushes.DarkGoldenrod;
    private static System.Drawing.Brush AttackColor = System.Drawing.Brushes.DarkRed;
    private static System.Drawing.Brush HealthColor = System.Drawing.Brushes.DarkBlue;

    /// <summary>
    /// Creates a png file of the given card at the given path.
    /// </summary>
    public static void GenerateImage(Card c, string path)
    {
        WriteableBitmap bitmap = null;

        switch (c.Type)
        {
            case CardType.Minion:
                bitmap = DrawMinion(c as Minion);
                break;

            case CardType.Spell:
                bitmap = DrawSpell(c as Spell);
                break;
        }

        // Convert to Bitmap and Save
        MemoryStream outStream = new MemoryStream();
        BitmapEncoder enc = new BmpBitmapEncoder();
        enc.Frames.Add(BitmapFrame.Create((BitmapSource)bitmap));
        enc.Save(outStream);
        Bitmap bmp = new Bitmap(outStream);
        path += Program.FileNameForCard(c.Name);
        bmp.Save(path, ImageFormat.Png);
    }

    private static WriteableBitmap DrawMinion(Minion m)
    {
        BitmapImage bitmapImage = null;
        if (m.Class == "Pflanze") bitmapImage = new BitmapImage(new Uri(Program.SourcePath + "Templates/template_minion_plant.png", UriKind.Absolute));
        else if (m.Class == "Dämon") bitmapImage = new BitmapImage(new Uri(Program.SourcePath + "Templates/template_minion_demon.png", UriKind.Absolute));
        else if (m.Class == "Drache") bitmapImage = new BitmapImage(new Uri(Program.SourcePath + "Templates/template_minion_drache.png", UriKind.Absolute));
        else if (m.Class == "Froschling") bitmapImage = new BitmapImage(new Uri(Program.SourcePath + "Templates/template_minion_froschling.png", UriKind.Absolute));
        else if (m.Class == "Geist") bitmapImage = new BitmapImage(new Uri(Program.SourcePath + "Templates/template_minion_geist.png", UriKind.Absolute));
        else if (m.Class == "Katzenkämpfer") bitmapImage = new BitmapImage(new Uri(Program.SourcePath + "Templates/template_minion_kk.png", UriKind.Absolute));
        else if (m.Class == "Stein") bitmapImage = new BitmapImage(new Uri(Program.SourcePath + "Templates/template_minion_stein.png", UriKind.Absolute));
        else if (m.Class == "Zwerg") bitmapImage = new BitmapImage(new Uri(Program.SourcePath + "Templates/template_minion_zwerg.png", UriKind.Absolute));

        else if (m.Class == "Alles") bitmapImage = new BitmapImage(new Uri(Program.SourcePath + "Templates/template_minion_all.png", UriKind.Absolute));
        else if (m.Class == "Zwerg / Froschling") bitmapImage = new BitmapImage(new Uri(Program.SourcePath + "Templates/template_minion_zwergfrosch.png", UriKind.Absolute));
        else if (m.Class == "Dinodrache / Katzenkämpfer") bitmapImage = new BitmapImage(new Uri(Program.SourcePath + "Templates/template_minion_drachekk.png", UriKind.Absolute));
        else
        {
            if (m.Class != "") Console.WriteLine("ERROR: Klasse " + m.Class + " vom Minion " + m.Name + " hat kein Template. Verwende Standard-Template.");
            bitmapImage = new BitmapImage(new Uri(Program.SourcePath + "Templates/template_minion.png", UriKind.Absolute));
        }
        WriteableBitmap bitmap = new WriteableBitmap(bitmapImage);

        DrawPlayableCardBasics(m, bitmap);
        DrawMinionStats(m, bitmap);
        return bitmap;
    }

    private static WriteableBitmap DrawSpell(Spell s)
    {
        BitmapImage bitmapImage = new BitmapImage(new Uri(Program.SourcePath + "Templates/template_spell.png", UriKind.Absolute));
        WriteableBitmap bitmap = new WriteableBitmap(bitmapImage);

        DrawPlayableCardBasics(s, bitmap);
        return bitmap;
    }

    private static void DrawPlayableCardBasics(PlayableCard c, WriteableBitmap bitmap)
    {
        // Name
        DrawText(bitmap, c.Name, Font, TitleFontSize, System.Drawing.FontStyle.Bold, TextColor, CardWidth / 2 - TitleWidth / 2, TitleY, TitleWidth, TitleHeight);

        // Cost
        DrawText(bitmap, c.Cost.ToString(), Font, CostFontSize, System.Drawing.FontStyle.Regular, CostColor, CostX, CostY);

        // Type
        DrawText(bitmap, c.Acronym, Font, TypeFontSize, System.Drawing.FontStyle.Regular, TextColor, TypeX, TypeY);

        // Text
        int textBoxHeight = c.Type == CardType.Minion ? TextBoxHeight_Minion : TextBoxHeight_Spell;
        DrawText(bitmap, c.Text, Font, TextFontSize, System.Drawing.FontStyle.Regular, TextColor, TextBoxX, TextBoxY, TextBoxWidth, textBoxHeight, StringAlignment.Center);

        // Picture
        string imagePath = Program.SourcePath + "Images/" + Program.FileNameForCard(c.Name);
        if (File.Exists(imagePath)) DrawImage(bitmap, imagePath, CardImageX, CardImageY);
    }

    private static void DrawMinionStats(Minion m, WriteableBitmap bitmap)
    {
        // Attack
        DrawText(bitmap, m.Attack.ToString(), Font, AttackFontSize, System.Drawing.FontStyle.Regular, AttackColor, AttackX, AttackY);

        // Health
        DrawText(bitmap, m.Health.ToString(), Font, HealthFontSize, System.Drawing.FontStyle.Regular, HealthColor, HealthX, HealthY);

        // Class
        int classFontSize = m.Class.Length <= 15 ? ClassFontSizeBig : ClassFontSizeSmall;
        DrawText(bitmap, m.Class, Font, classFontSize, System.Drawing.FontStyle.Regular, TextColor, CardWidth / 2 - ClassWidth / 2, ClassY, ClassWidth, ClassHeight);
        
    }

    private static void DrawText(WriteableBitmap bitmap, string text, string fontName, int fontSize, System.Drawing.FontStyle fontStyle, System.Drawing.Brush color, int x, int y, int width = 0, int height = 0, StringAlignment verticalAlignment = StringAlignment.Center)
    {
        var w = bitmap.PixelWidth;
        var h = bitmap.PixelHeight;
        var stride = bitmap.BackBufferStride;
        var pixelPtr = bitmap.BackBuffer;

        // this is fast, changes to one object pixels will now be mirrored to the other 
        var bm2 = new Bitmap(w, h, stride, System.Drawing.Imaging.PixelFormat.Format32bppRgb, pixelPtr);

        bitmap.Lock();

        // you might wanna use this in combination with Lock / Unlock, AddDirtyRect, Freeze
        // before you write to the shared Ptr
        StringFormat format = new StringFormat()
        {
            LineAlignment = verticalAlignment,
            Alignment = StringAlignment.Center,
            Trimming = StringTrimming.EllipsisCharacter,
        };
        Font font = new Font(fontName, fontSize, fontStyle);

        using (var g = Graphics.FromImage(bm2))
        {
            if(width == 0 && height == 0) g.DrawString(text, font, color, x, y, format);
            else g.DrawString(text, font, System.Drawing.Brushes.Black, new Rectangle(x, y, width, height), format);
        }

        bitmap.AddDirtyRect(new Int32Rect(0, 0, 200, 100));
        bitmap.Unlock();
    }

    private static void DrawImage(WriteableBitmap bitmap, string imagePath, int x, int y)
    {
        BitmapSource source = new BitmapImage(new Uri(imagePath));

        int sourceBytesPerPixel = GetBytesPerPixel(source.Format);
        int sourceBytesPerLine = source.PixelWidth * sourceBytesPerPixel;

        byte[] sourcePixels = new byte[sourceBytesPerLine * source.PixelHeight];
        source.CopyPixels(sourcePixels, sourceBytesPerLine, 0);

        Int32Rect sourceRect = new Int32Rect(x, y, source.PixelWidth, source.PixelHeight);
        bitmap.WritePixels(sourceRect, sourcePixels, sourceBytesPerLine, 0);
    }

    private static int GetBytesPerPixel(System.Windows.Media.PixelFormat format)
    {
        return format.BitsPerPixel / 8;
    }
}

