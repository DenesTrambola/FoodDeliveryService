using FoodDeliveryService.Domain.Entities;
using FoodDeliveryService.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryService.Infrastructure.Persistence.Data;

public class FoodDeliveryDbContext : DbContext
{
    public DbSet<Meal> Meals { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Restaurant> Restaurants { get; set; } = null!;

    public FoodDeliveryDbContext(DbContextOptions<FoodDeliveryDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MealConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new RestaurantConfiguration());
        modelBuilder.ApplyConfiguration(new MealInOrderConfiguration());
    }
}
