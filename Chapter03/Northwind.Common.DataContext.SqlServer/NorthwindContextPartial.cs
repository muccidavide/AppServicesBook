using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Northwind.EntityModels
{
    public partial class NorthwindContext : DbContext
    {
        private static readonly SetLastRefreshedInterceptor setLastRefreshedInterceptor = new();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                SqlConnectionStringBuilder builder = new();
                builder.DataSource = "ASUSDAVIDE\\SQLEXPRESS";
                builder.InitialCatalog = "Northwind";
                builder.TrustServerCertificate = true;
                builder.MultipleActiveResultSets = true;
                builder.ConnectTimeout = 3;
                builder.IntegratedSecurity = true;
                optionsBuilder.UseSqlServer(builder.ConnectionString);
            }
            optionsBuilder.AddInterceptors(setLastRefreshedInterceptor);
        }
    }
}

