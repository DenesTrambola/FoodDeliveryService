using FoodDeliveryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodDeliveryService.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.TotalPrice).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(o => o.Status).IsRequired();
        builder.HasOne(o => o.Restaurant)
            .WithMany(r => r.Orders)
            .HasForeignKey(o => o.RestaurantId);
        builder.HasMany(o => o.MealInOrders)
            .WithOne(m => m.Order)
            .HasForeignKey(m => m.OrderId);
    }
}
