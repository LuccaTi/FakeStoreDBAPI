using FakeStoreDBAPI.Host.Models;
using FakeStoreDBAPI.Host.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FakeStoreDBAPI.Host.Data
{
    public class FakeStoreDbContext : DbContext
    {
        public FakeStoreDbContext(DbContextOptions<FakeStoreDbContext> options) : base(options)
        {

        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is IAuditable && (
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var auditableEntity = (IAuditable)entityEntry.Entity;
                auditableEntity.DateUpdate = DateTime.UtcNow;

                if(entityEntry.State == EntityState.Added)
                {
                    auditableEntity.DateCreate = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
