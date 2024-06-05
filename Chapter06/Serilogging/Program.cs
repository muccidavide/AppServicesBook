using Serilog; // To use Log, LoggerConfiguration, RollingInterval.
using Serilog.Core; // To use Logger.
using Serilogging.Models; // To use ProductPageView.

using Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();


Log.Logger = log;
Log.Information("The global logger is been configured");
Log.Warning("Danger, Serilog danger");
Log.Error("Error by Serilog");
Log.Fatal("This is a fatal error");

ProductPageView productPageView = new ProductPageView()
{
    PageTitle = "Chai",
    SiteSection = "Beverages",
    ProductId = 1
};

Log.Information("{@PageView} occured at {viewed}", 
    productPageView, DateTimeOffset.UtcNow);


Log.CloseAndFlush();    