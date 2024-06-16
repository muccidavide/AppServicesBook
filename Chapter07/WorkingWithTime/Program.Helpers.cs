using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

partial class Program
{
    private static void ConfigureConsole(string culture = "en-US",
        bool overrideComputerCulture = true)
    {
        // TO enable special charachters like Euro currency symbol
        OutputEncoding = Encoding.UTF8;
        Thread t = Thread.CurrentThread;
        if (overrideComputerCulture)
        {
            t.CurrentCulture = CultureInfo.GetCultureInfo(culture);
            t.CurrentUICulture = t.CurrentCulture;
        }

        CultureInfo ci = t.CurrentCulture;
        WriteLine($"Current Culture: {ci.DisplayName}");
        WriteLine($"Short date pattern: {ci.DateTimeFormat.ShortDatePattern}");
        WriteLine($"Long date pattern: {ci.DateTimeFormat.LongDatePattern}");
        WriteLine();
    }

    private static void SectionTitle(string title)
    {
        ConsoleColor previousColor = ForegroundColor;
        ForegroundColor = ConsoleColor.DarkYellow;
        WriteLine($"*** {title}");
        ForegroundColor = previousColor;
    }
}

