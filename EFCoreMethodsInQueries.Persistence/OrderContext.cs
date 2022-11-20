using EFCoreMethodsInQueries.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCoreMethodsInQueries.Persistence;

public class OrderContext : DbContext
{
    public OrderContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}