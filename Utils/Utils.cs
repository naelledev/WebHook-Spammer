using System;
internal class Utils
{
   public static string CenterText(string text)
   {
        int num = (Console.WindowWidth - text.Length) / 2;
        return text.PadLeft(num + text.Length).PadRight(Console.WindowWidth);
   }
}

