using EFCoreMethodsInQueries.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreMethodsInQueries.Persistence;

public class OrderConfigurations : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.OrderDate);
        builder.Property(o => o.State)
            .HasConversion(
                v => v.ToString(),
                v => (OrderState)Enum.Parse(typeof(OrderState), v));
    }
}