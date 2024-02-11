using Microsoft.Data.SqlClient; // To use SqlConnectionStringBuilder.
using Microsoft.EntityFrameworkCore;
using Northwind.Console.HierarchyMapping.Models; // GenerateCreateScript()

DbContextOptionsBuilder<HierarchyDb> options = new();
SqlConnectionStringBuilder builder = new();

builder.DataSource = "ASUSDAVIDE\\SQLEXPRESS"; 
builder.InitialCatalog = "HierarchyMapping";
builder.TrustServerCertificate = true;
builder.MultipleActiveResultSets = true;
builder.ConnectTimeout = 3;
builder.IntegratedSecurity = true;

options.UseSqlServer(builder.ConnectionString);

using(HierarchyDb db = new(options.Options))
{
    bool deleted = await db.Database.EnsureDeletedAsync();
    WriteLine($"Database deleted: {deleted}");

    bool created = await db.Database.EnsureCreatedAsync();
    WriteLine($"Database created: {created}");

    WriteLine($"SQl script used to create the db:");
    WriteLine(db.Database.GenerateCreateScript());

    if(db.Students is null || !db.Students.Any())
    {
        WriteLine("There's no students.");
    }
    else
    {
        foreach(var student in db.Students)
        {
            WriteLine("{0} studies {1}",
                student.Name, student.Subject);
        }
    }

    if(db.Employees is null ||  !db.Employees.Any())
    {
        WriteLine("There's no Employee");
    }
    else
    {
        foreach (var employee in db.Employees)
        {
            WriteLine("{0} was hired on {1}",
                employee.Name, employee.HireDate);
        }
    }

    if (db.People is null || !db.People.Any())
    {
        WriteLine("There are no people.");
    }
    else
    {
        foreach (Person person in db.People)
        {
            WriteLine("{0} has ID of {1}",
            person.Name, person.Id);
        }
    }

}

