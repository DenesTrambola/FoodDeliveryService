using FoodDeliveryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodDeliveryService.Infrastructure.Persistence.Configurations;

public class MealInOrderConfiguration : IEntityTypeConfiguration<MealInOrder>
{
    public void Configure(EntityTypeBuilder<MealInOrder> builder)
    {
        builder.HasKey(m => new { m.MealId, m.OrderId });
        builder
            .HasOne(m => m.Meal)
            .WithMany(m => m.MealInOrders)
            .HasForeignKey(m => m.MealId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne(m => m.Order)
            .WithMany(o => o.MealInOrders)
            .HasForeignKey(m => m.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
