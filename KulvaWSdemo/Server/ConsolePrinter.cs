using System;

namespace KulvaServer
{
    internal static class Printer
    {
        internal static string CurrentTimeString
        {
            get { return DateTimeString(DateTime.Now, "HH:mm:ss"); }
        }

        internal static string DateTimeString(DateTime dt, string format = "yyyy-MM-dd HH:mm")
        {
            return dt.ToString(format, System.Globalization.CultureInfo.InvariantCulture);
        }

        internal static void Colored(string t, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(t);
            //Console.ResetColor();
        }

        internal static void Colored(string t1, string t2, ConsoleColor color, bool newline = true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(t1);
            Console.ForegroundColor = color;
            if (newline) { Console.WriteLine(t2); } else { Console.Write(t2); }
            //Console.ResetColor(); //kunhan resetoidaan ennen exittiä niin homma ok
        }
    }
}