using System.Globalization;
using Humanizer;
using Humanizer.Inflections; // To use Vocabularies.
using Packt.Shared;


partial class Program
{
    private static void ConfigureConsole(string culture = "en_US")
    {
        // to enable special characters like ... (ellipsis) as a single character
        OutputEncoding = System.Text.Encoding.UTF8;

        Thread t = Thread.CurrentThread;
        t.CurrentCulture = CultureInfo.GetCultureInfo(culture);
        t.CurrentUICulture = t.CurrentCulture;

        WriteLine("Current Culture: {0}", t.CurrentCulture.DisplayName);
        WriteLine();
    }

    private static void OutputCasings(string original)
    {
        WriteLine("Original casing: {0}", original);
        WriteLine("Lower casing: {0}", original.Transform(To.LowerCase));
        WriteLine("Upper casing: {0}", original.Transform(To.UpperCase));
        WriteLine("Title casing: {0}", original.Transform(To.TitleCase));
        WriteLine("Sentence casing: {0}", original.Transform(To.SentenceCase));
        WriteLine("Lower, then Sentence casing: {0}", original.Transform(To.LowerCase, To.SentenceCase));
        WriteLine();

    }

    private static void OutputSpacingAndDashes()
    {
        string ugly = "ERROR_MESSAGE_FROM_SERVICE";
        WriteLine("Oringal string: {0}", ugly);
        WriteLine("Humanized: {0}", ugly.Humanize());

        // LetterCasing is Legacy and will be removed in future
        WriteLine("Humanized, lower case : {0}", ugly.Humanize(LetterCasing.LowerCase));

        //Use Transform for casing instead
        WriteLine("Transformed (lower case then sentence case): {0}", ugly.Transform(To.LowerCase, To.SentenceCase));

        WriteLine("Humanized, Transformed (lower case, then sentence case): {0}", ugly.Humanize().Transform(To.LowerCase, To.SentenceCase));
    }

    private static void OutputEnumNames()
    {
        var favoriteAncientWander = WondersOfTheAncientWorld.StatueOfZeusAtOlympia;
        WriteLine("Raw enum value name: {0}", favoriteAncientWander);
        WriteLine("Humanized: {0}", favoriteAncientWander.Humanize());

        WriteLine("Humanized, then Titleized: {0}", favoriteAncientWander.Humanize().Titleize());
        WriteLine("Truncated to 8 characters: {0}", favoriteAncientWander.ToString().Truncate(8));
        WriteLine("Kebaberized: {0}", favoriteAncientWander.ToString().Kebaberize());
    }

    private static void NumberFormatting()
    {
        Vocabularies.Default.AddIrregular("biceps", "bicepses");
        Vocabularies.Default.AddIrregular("attorney general", "attorneys general");
        int number = 123;

        WriteLine($"Oringinal number: {number}");
        WriteLine($"Roman: {number.ToRoman()}");
        WriteLine($"Words: {number.ToWords()}");
        WriteLine($"Ordinal words: {number.ToOrdinalWords()}");

        string[] things = { "fox", "person", "sheep",
            "apple", "goose", "oasis", "potato", "die", "dwarf",
            "attorney general", "biceps"};

        for(int i = 1; i <= 3; i++)
        {
            for(int j = 0; j < things.Length; j++)
            {
                Write(things[j].ToQuantity(i, ShowQuantityAs.Words));

                if (j < things.Length - 1) Write(", ");
            }
            WriteLine();

            int thousands = 12345;
            int millions = 123456789;
            
            WriteLine("Original: {0}, Metric: About {1}", thousands, thousands.ToMetric(decimals:0));

            WriteLine("Original: {0}, Metric: {1}", thousands, 
                thousands.ToMetric(MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord, 
                decimals: 0));

            WriteLine("Original: {0}, Metric: {1}",millions, millions.ToMetric(decimals:1));
        }
    }

    private static void DateTimeFormatting()
    {
        DateTimeOffset now = DateTimeOffset.Now;
        
        // by default, all Humanizer comparisons are to Now (UTC)
        WriteLine($"Now (UTC): {now}");

        WriteLine("Add 3 hours, Humanized: {0}",
            now.AddHours(3).Humanize());

        WriteLine("Add 3 hours and 1 minute, Humanized: {0}",
            now.AddHours(3).AddMinutes(1).Humanize());

        WriteLine("Subtract 3 hours, Humanized: {0}", 
            now.AddHours(-3).Humanize());

        WriteLine("Add 24 hours, Humanized: {0}", 
            now.AddHours(24).Humanize());

        WriteLine("Add 25 hours, Humanized: {0}", 
            now.AddHours(25).Humanize());

        WriteLine("Add 7 days, Humanized: {0}", 
            now.AddDays(7).Humanize());

        WriteLine("Add 7 days and 1 minute, Humanized: {0}", 
            now.AddDays(7).AddMinutes(1).Humanize());

        WriteLine("Add 1 month, Humanized: {0}",
            now.AddMonths(1).Humanize());

        WriteLine();

        //Example of Timespan Humanization
        int[] daysArray = { 12, 13, 14, 15, 16 };

        foreach (var days in daysArray)
        {
            WriteLine("{0} days, Humanized: {1}",
                days, TimeSpan.FromDays(days).Humanize());

            WriteLine("{0} days, Humanized with precision 2: {1}", 
                days, TimeSpan.FromDays(days).Humanize(precision:2));

            WriteLine("{0} days, Humanized with max unit days: {1}",
                days, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Day));                          

            WriteLine();
        }

        // Example of clock notation
        TimeOnly[] times =
        {
            new TimeOnly(9, 0),
            new TimeOnly(9, 15),
            new TimeOnly(15, 30)
        };

        foreach (var time in times)
        {
            WriteLine("{0}: {1}", time, time.ToClockNotation());
        }
    }
}

