using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FakeStoreDBAPI.Host.Data
{
    /// <summary>
    /// Factory for creating DbContext instances at design-time.
    /// This is used by EF Core tools (migrations) and ensures the application doesn't need to run.
    /// </summary>
    public class FakeStoreDbContextFactory : IDesignTimeDbContextFactory<FakeStoreDbContext>
    {
        public FakeStoreDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found in appsettings.json");
            }

            var optionsBuilder = new DbContextOptionsBuilder<FakeStoreDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new FakeStoreDbContext(optionsBuilder.Options);
        }
    }
}
