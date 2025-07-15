// Orm.DAL/Configurations/OrderItemConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orm.Core.Entities;

namespace Orm.DAL.Configurations;

public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(oi => oi.Quantity)
            .IsRequired();

        builder.Property(oi => oi.TotalAmount)
            .HasColumnType("decimal(18,2)") // Ensures decimal type in DB for precision
            .IsRequired();

        // Configure relationship with Order
        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade); // If an Order is deleted, its OrderItems are also deleted

        // Configure relationship with MenuItem
        builder.HasOne(oi => oi.MenuItem)
            .WithMany() // MenuItem can be in many OrderItems, but MenuItem entity itself doesn't need a collection navigation to OrderItem
            .HasForeignKey(oi => oi.MenuItemId)
            .OnDelete(DeleteBehavior.Restrict); // Prevents MenuItem from being deleted if it's part of an OrderItem to maintain data integrity
    }
}