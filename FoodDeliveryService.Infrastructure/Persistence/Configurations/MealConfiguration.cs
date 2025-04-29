using FoodDeliveryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodDeliveryService.Infrastructure.Persistence.Configurations;

public class MealConfiguration : IEntityTypeConfiguration<Meal>
{
    public void Configure(EntityTypeBuilder<Meal> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Name).IsRequired().HasMaxLength(64);
        builder.Property(m => m.Price).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(m => m.Description).IsRequired().HasMaxLength(256);
        builder.HasOne(m => m.Restaurant)
            .WithMany(r => r.Meals)
            .HasForeignKey(m => m.RestaurantId);
        builder.HasMany(m => m.MealInOrders)
            .WithOne(m => m.Meal)
            .HasForeignKey(m => m.MealId);
    }
}
