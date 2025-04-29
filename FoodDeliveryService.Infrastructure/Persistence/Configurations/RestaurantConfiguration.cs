using FoodDeliveryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodDeliveryService.Infrastructure.Persistence.Configurations;

public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Name).IsRequired().HasMaxLength(64);
        builder.Property(r => r.PhoneNumber).IsRequired().HasMaxLength(15);
        builder.HasMany(r => r.Meals)
            .WithOne(m => m.Restaurant)
            .HasForeignKey(m => m.RestaurantId);
        builder.HasMany(r => r.Orders)
            .WithOne(o => o.Restaurant)
            .HasForeignKey(o => o.RestaurantId);
    }
}
