using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orm.Core.Entities;

namespace Orm.DAL.Configurations;

public class OrderConfig:IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Count).IsRequired();
        builder.Property(x => x.OrderDate).IsRequired().HasDefaultValueSql("GETDATE()");

        builder.HasMany(x => x.OrderItems)
            .WithOne(xi => xi.Orders)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}