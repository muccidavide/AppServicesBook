using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Northwind.EntityModels;

namespace Northwind.EntityModels
{
    public static class NorthwindContextExtensions
    {

        /// <summary>
        /// Adds NorthwindContext to the specified IServiceCollection. Uses the SqlServer database provider.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="connectionString">Set to override the default.</param>
        /// <returns>An IServiceCollection that can be used to add more services.</returns>
        public static IServiceCollection AddNorthwindContext(
            this IServiceCollection services,
            string? connectionString)
        {
            if (connectionString == null)
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "ASUSDAVIDE\\SQLEXPRESS";
                builder.InitialCatalog = "Northwind";
                builder.TrustServerCertificate = true;
                builder.MultipleActiveResultSets = true;
                builder.ConnectTimeout = 3;
                builder.IntegratedSecurity = true;

                connectionString = builder.ConnectionString;
            }

            services.AddDbContext<NorthwindContext>(options =>
            {
                options.UseSqlServer(connectionString);

                // Log to console when excuting EF core commands
                options.LogTo(Console.WriteLine,
                    new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting });
            },
            contextLifetime: ServiceLifetime.Transient,
            optionsLifetime: ServiceLifetime.Transient);

            return services;

        }
    }
}
