using System.Globalization; // To use CultureInfo.
using System.Text; // To use Encoding.
OutputEncoding = Encoding.UTF8; // To enable Euro symbol output.
// Simulate French culture to test Euro currency symbol output.
Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
//await CreateCosmosResources();
await CreateProductItems();
//await ListProductItems();
//await DeleteProductItems();