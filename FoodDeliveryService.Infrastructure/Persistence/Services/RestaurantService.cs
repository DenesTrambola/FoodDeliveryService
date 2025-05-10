using ErrorOr;
using FoodDeliveryService.Application.Services;
using FoodDeliveryService.Domain.Entities;
using FoodDeliveryService.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FoodDeliveryService.Infrastructure.Persistence.Services;

public class RestaurantService(FoodDeliveryDbContext context, ILogger<RestaurantService> logger) : IRestaurantService
{
    private readonly FoodDeliveryDbContext _context = context;
    private readonly ILogger<RestaurantService> _logger = logger;

    public async Task<ErrorOr<Restaurant>> CreateRestaurantAsync(Restaurant restaurant, CancellationToken cancellationToken = default)
    {
        var existingRestaurant = await _context.Restaurants
            .FirstOrDefaultAsync(r => r.Name == restaurant.Name, cancellationToken);
        if (existingRestaurant is not null)
            return Error.Conflict($"Restaurant with name {restaurant.Name} already exists.");

        await _context.Restaurants.AddAsync(restaurant, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Restaurant created: {RestaurantId}", restaurant.Id);

        return restaurant;
    }

    public async Task<ErrorOr<Deleted>> DeleteRestaurantAsync(Guid restaurantId, CancellationToken cancellationToken = default)
    {
        var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.Id == restaurantId, cancellationToken);

        if (restaurant is null)
            return Error.NotFound();

        _context.Restaurants.Remove(restaurant);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Restaurant deleted: {RestaurantId}", restaurantId);

        return new Deleted();
    }

    public async Task<ErrorOr<IEnumerable<Restaurant>>> GetAllRestaurantsAsync(CancellationToken cancellationToken = default)
    {
        var restaurants = await _context.Restaurants
            .Include(r => r.Meals)
            .ToListAsync(cancellationToken);

        return restaurants;
    }

    public async Task<ErrorOr<Restaurant>> GetRestaurantByIdAsync(Guid restaurantId, CancellationToken cancellationToken = default)
    {
        var restaurant = await _context.Restaurants
            .Include(r => r.Meals)
            .FirstOrDefaultAsync(r => r.Id == restaurantId, cancellationToken);

        if (restaurant is null)
            return Error.NotFound();

        return restaurant;
    }

    public async Task<ErrorOr<Restaurant>> UpdateRestaurantAsync(Restaurant restaurant, CancellationToken cancellationToken = default)
    {
        var existingRestaurant = await _context.Restaurants
            .Include(r => r.Meals)
            .FirstOrDefaultAsync(r => r.Id == restaurant.Id, cancellationToken);

        if (existingRestaurant is null)
            return Error.NotFound();

        existingRestaurant.Name = restaurant.Name;
        existingRestaurant.PhoneNumber = restaurant.PhoneNumber;
        existingRestaurant.Email = restaurant.Email;
        existingRestaurant.Description = restaurant.Description;

        _context.Restaurants.Update(existingRestaurant);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Restaurant updated: {RestaurantId}", restaurant.Id);

        return existingRestaurant;
    }
}
