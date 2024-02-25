using Microsoft.Azure.Cosmos; // To use CosmosClient and so on.
using System.Net; // To use HttpStatusCode.
using Northwind.EntityModels; // To use NorthwindContext and so on.
using Northwind.CosmosDb.Items; // To use ProductCosmos and so on.
using Microsoft.EntityFrameworkCore; // To use Include extension method.

partial class Program
{
    private static string endpointUri = "https://localhost:8081/";
    private static string primaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHL\r\nM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

    static async Task CreateCosmosResourcess()
    {
        SectionTitle("Creating Cosmos resources");

        try
        {
            using (CosmosClient client = new(accountEndpoint: endpointUri,
                authKeyOrResourceToken: primaryKey))
            {
                DatabaseResponse dbResponse = await client.CreateDatabaseIfNotExistsAsync("Northwind", throughput: 400 /* RU/s */);

                string status = dbResponse.StatusCode switch
                {
                    HttpStatusCode.OK => "exsists",
                    HttpStatusCode.Created => "created",
                    _ => "unknown"
                };

                WriteLine("Database Id: {0}, Status: {1}.",
                    arg0: dbResponse.Database.Id, arg1: status);

                IndexingPolicy indexingPolicy = new()
                {
                    IndexingMode = IndexingMode.Consistent,
                    Automatic = true, // Items are indexed unless explicitl excluded.
                    IncludedPaths = { new IncludedPath { Path = "/*" } }
                };

                ContainerProperties containerProperties = new("Products",
                partitionKeyPath: "/productId")
                {
                    IndexingPolicy = indexingPolicy
                };

                ContainerResponse containerResponse = await dbResponse.Database
                .CreateContainerIfNotExistsAsync(
                containerProperties, throughput: 1000 /* RU/s */);
                status = dbResponse.StatusCode switch
                {
                    HttpStatusCode.OK => "exists",
                    HttpStatusCode.Created => "created",
                    _ => "unknown",
                };


                WriteLine("Container Id: {0}, Status: {1}.",
                arg0: containerResponse.Container.Id, arg1: status);

                Container container = containerResponse.Container;
                ContainerProperties properties = await container.

                ReadContainerAsync();
                WriteLine($" PartitionKeyPath: {properties.PartitionKeyPath}");
                WriteLine($" LastModified: {properties.LastModified}");
                WriteLine(" IndexingPolicy.IndexingMode: {0}",
                arg0: properties.IndexingPolicy.IndexingMode);

                WriteLine(" IndexingPolicy.IncludedPaths: {0}",
                arg0: string.Join(",", properties.IndexingPolicy
                .IncludedPaths.Select(path => path.Path)));
                WriteLine($" IndexingPolicy: {properties.IndexingPolicy}");
            }
        }
        catch (HttpRequestException ex)
        {
            WriteLine($"Error: {ex.Message}");
            WriteLine("Hint: If you are using the Azure Cosmos Emulator then please make sure that it is running.");
        }
        catch (Exception ex)
        {
            WriteLine("Error: {0} says {1}",
            arg0: ex.GetType(),
            arg1: ex.Message);
        }
    }

    static async Task CreateProductItems()
    {

        SectionTitle("Create products items");

        double totalCharge = 0.0;
        try
        {
            using (CosmosClient client = new(accountEndpoint: endpointUri, authKeyOrResourceToken: primaryKey))
            {
                Container container = client.GetContainer(
                    databaseId: "Northwind", containerId: "Products"
                    );

                using (NorthwindContext db = new())
                {
                    if (!db.Database.CanConnect())
                    {
                        WriteLine("Cannot connect to the SQL Server database to " +
                                " read products using database connection string: " +
                         db.Database.GetConnectionString());
                        return;
                    }

                    ProductCosmos[] products = db.Products.Include(p => p.Category).Include(p => p.Supplier)
                        .Where(p => p.Category != null && p.Supplier != null)
                        .Select(p => new ProductCosmos
                        {
                            id = p.ProductId.ToString(),
                            productId = p.ProductId.ToString(),
                            productName = p.ProductName,
                            quantityPerUnit = p.QuantityPerUnit,
                            category = p.Category == null ? null : new CategoryCosmos
                            {
                                categoryId = p.Category.CategoryId,
                                categoryName = p.Category.CategoryName,
                                description = p.Category.Description
                            },
                            supplier = p.Supplier == null ? null : new SupplierCosmos
                            {
                                supplierId = p.Supplier.SupplierId,
                                companyName = p.Supplier.CompanyName,
                                contactName = p.Supplier.ContactName,
                                contactTitle = p.Supplier.ContactTitle,
                                address = p.Supplier.Address,
                                city = p.Supplier.City,
                                country = p.Supplier.Country,
                                postalCode = p.Supplier.PostalCode,
                                region = p.Supplier.Region,
                                phone = p.Supplier.Phone,
                                fax = p.Supplier.Fax,
                                homePage = p.Supplier.HomePage
                            },
                            unitPrice = p.UnitPrice,
                            unitsInStock = p.UnitsInStock,
                            reorderLevel = p.ReorderLevel,
                            unitsOnOrder = p.UnitsOnOrder,
                            discontinued = p.Discontinued
                        }).ToArray();

                    foreach (var product in products)
                    {
                        try
                        {
                            ItemResponse<ProductCosmos> productResponse = await container.ReadItemAsync<ProductCosmos>(id: product.id, new PartitionKey(product.productId));
                            WriteLine("Item with id: {0} exists. Query consumed {1} RUs.",
                              productResponse.Resource.id, productResponse.
                              RequestCharge);

                            totalCharge += productResponse.RequestCharge;
                        }
                        catch (CosmosException ex)
                        when (ex.StatusCode == HttpStatusCode.NotFound)
                        {
                            ItemResponse<ProductCosmos> productResponse = await container.CreateItemAsync(product);

                            WriteLine("Created item with id: {0}. Insert consumed {1}vRUs.",
                                productResponse.Resource.id, productResponse.
                                RequestCharge);

                            totalCharge += productResponse.RequestCharge;
                        }
                        catch (Exception ex)
                        {
                            WriteLine("Error: {0} says {1}",
                            arg0: ex.GetType(),
                            arg1: ex.Message);
                        }
                    }


                };

            };
        }
        catch (HttpRequestException ex)
        {
            WriteLine("Error: {0} says {1}",
                arg0: ex.GetType(),
                arg1: ex.Message);
        }
        WriteLine("Total requests charge: {0:N2} RUs", totalCharge);
    }

    static async Task ListProductItems(string sqlText = "SELECT * FROM c")
    {
        SectionTitle("Listing products items");

        try
        {
            using (CosmosClient client = new(
                accountEndpoint: endpointUri, authKeyOrResourceToken: primaryKey))
            {
                Container container = client.GetContainer(
                    databaseId: "Northwind", containerId: "Products"
                    );

                WriteLine("Runnin query {0}", sqlText);

                QueryDefinition query = new(sqlText);

                using FeedIterator<ProductCosmos> resultIterator = container.GetItemQueryIterator<ProductCosmos>(query);

                if (!resultIterator.HasMoreResults)
                {
                    WriteLine("No results found");
                }

                while (resultIterator.HasMoreResults)
                {
                    FeedResponse<ProductCosmos> products = await resultIterator.ReadNextAsync();

                    WriteLine("Status Code: {0}, Request charge: {1} RUs",
                        products.StatusCode, products.RequestCharge);

                    WriteLine($"{products.Count} products found");

                    foreach (ProductCosmos product in products)
                    {
                        WriteLine("id: {0}, productName: {1}, unitPrice: {2}",
                            arg0: product.id, arg1: product.productName,
                            arg2: product.unitPrice.ToString());
                    }
                }


            }
        }
        catch (HttpRequestException ex)
        {
            WriteLine($"Error: {ex.Message}");
            WriteLine("Hint: If you are using the Azure Cosmos Emulator then please make sure it is running.");
        }
        catch (Exception ex)
        {
            WriteLine("Error: {0} says {1}",
            arg0: ex.GetType(),
            arg1: ex.Message);
        }
    }

    static async Task DeleteProductItems()
    {
        SectionTitle("Delete Products");

        double totalCharge = 0.0;

        try
        {
            using (CosmosClient client = new(accountEndpoint: endpointUri, authKeyOrResourceToken: primaryKey))
            {
                Container container = client.GetContainer("Northwind", "Products");

                string sqlText = "SELECT * FROM c";

                WriteLine("Running query: {0}", sqlText);

                QueryDefinition query = new(sqlText);

                using FeedIterator<ProductCosmos> resultsIterator = container.GetItemQueryIterator<ProductCosmos>(query);

                while (resultsIterator.HasMoreResults)
                {
                    FeedResponse<ProductCosmos> products = await resultsIterator.ReadNextAsync();

                    foreach (var product in products)
                    {
                        WriteLine("Delete id: {0}, productName: {1}",
                            arg0: product.id, arg1: product.productName);

                        ItemResponse<ProductCosmos> response = await container.DeleteItemAsync<ProductCosmos>(id: product.id, partitionKey: new(product.id));
                        WriteLine("Status code: {0}, Request charge: {1} RUs.",
                            response.StatusCode, response.RequestCharge);
                            totalCharge += response.RequestCharge;
                    }
                }

            }
        }
        catch (HttpRequestException ex)
        {
            WriteLine($"Error: {ex.Message}");
            WriteLine("Hint: If you are using the Azure Cosmos Emulator then please make sure it is running.");
        }
        catch (Exception ex)
        {
            WriteLine("Error: {0} says {1}",
                arg0: ex.GetType(),
                arg1: ex.Message);
        }

        WriteLine("Total requests charge: {0:N2} RUs", totalCharge);
    }
}

