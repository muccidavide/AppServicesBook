using AutoMapper; // To use MapperConfiguration, IMapper.
using MappingObjects.Mappers; // To use CartToSummaryMapper.
using Northwind.EntityModels; // To use Customer, Cart, LineItem.
using Northwind.ViewModels; // To use Summary.
using System.Text; // To use Encoding.

// Set the console's output encoding to UTF-8 to support
// Unicode characters like the Euro currency symbol.
OutputEncoding = Encoding.UTF8;

Cart cart = new(Customer: new(
    FirstName: "John",
    LastName: "Smith"),
    Items: new(){
        new (ProductName: "Apples", UnitPrice: 0.49M, Quantity: 10),
        new (ProductName: "Bananas", UnitPrice: 0.99M, Quantity: 4)
    });

WriteLine("*** Original data before mapping");
WriteLine($"{cart.Customer}");
foreach (var item in cart.Items)
{
    WriteLine($"  {item}");
}

// Get mapper configuration for converting a cart to a summary
MapperConfiguration config = CartToSummaryMapper.GetMapperConfiguration();

// Create a mapper using configuration
IMapper mapper = config.CreateMapper();

// Perform mapping
Summary summary = mapper.Map<Cart, Summary>(cart);

// Output the results

WriteLine();
WriteLine("*** After mapping");
WriteLine($"Summary: {summary.FullName} spent {summary.Total:C}");