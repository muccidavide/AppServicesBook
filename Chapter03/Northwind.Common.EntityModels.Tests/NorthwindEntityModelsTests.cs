using Northwind.EntityModels;

namespace Northwind.Common.EntityModels.Tests
{
    public class NorthwindEntityModelsTests
    {
        [Fact]
        public void CanConnectIsTrue()
        {
            using(NorthwindContext db = new())
            {
                bool canConnect = db.Database.CanConnect();
                Assert.True(canConnect);
            }
        }

        [Fact]
        public void ProviderIsSqlServer()
        {
            using (NorthwindContext db = new())
            {
                string? provider = db.Database.ProviderName;
                Assert.Equal("Microsoft.EntityFrameworkCore.SqlServer", provider);
            }
        }

        [Fact]  
        public void ProductId1IsChai()
        {
            using (NorthwindContext db = new())
            {
                Employee employee1 = db.Employees.Single(p => p.EmployeeId == 1);

                DateTimeOffset now = DateTimeOffset.UtcNow;

                Assert.InRange(actual: employee1.LastRefreshed, low: now.Subtract(TimeSpan.FromSeconds(5)), high: now.AddSeconds(5));
            }
        }
    }
}