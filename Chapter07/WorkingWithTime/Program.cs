
    
/*ConfigureConsole(); // Defaults to en-US culture.
SectionTitle("Specifying date and time values");
WriteLine($"DateTime.MinValue: {DateTime.MinValue}");
WriteLine($"DateTime.MaxValue: {DateTime.MaxValue}");
WriteLine($"DateTime.UnixEpoch: {DateTime.UnixEpoch}");
WriteLine($"DateTime.Now: {DateTime.Now}");
WriteLine($"DateTime.Today: {DateTime.Today}");
WriteLine($"DateTime.Today: {DateTime.Today:d}");
WriteLine($"DateTime.Today: {DateTime.Today:D}");*/

DateTime xmas = new DateTime(year: 2024, month: 12, day: 25);
WriteLine($"Chirstmas (default format): {xmas}");
WriteLine($"Chirstmas (custom short format): {xmas:ddd d/M/yy}");
WriteLine($"Chirstmas (custom long format): {xmas:dddd, dd MMMM yyyy}");
WriteLine($"Chirstmas (standard long format): {xmas:D}");
WriteLine($"Chirstmas (sortable): {xmas:u}");
WriteLine($"Chirstmas month: {xmas.Month}");
WriteLine($"Chirstmas day: ");
WriteLine($"Chirstmas day {xmas.DayOfYear} of the year {xmas.Year} and Year");
WriteLine($"Chirstmas {xmas.Year} is on {xmas.DayOfWeek}");

// continue page 260
