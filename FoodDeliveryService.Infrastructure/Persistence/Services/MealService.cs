using ErrorOr;
using FoodDeliveryService.Application.Services;
using FoodDeliveryService.Domain.Entities;
using FoodDeliveryService.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FoodDeliveryService.Infrastructure.Persistence.Services;

public class MealService(FoodDeliveryDbContext context, ILogger logger) : IMealService
{
    private readonly FoodDeliveryDbContext _context = context;
    private readonly ILogger _logger = logger;

    public async Task<ErrorOr<Meal>> CreateMealAsync(Meal meal, CancellationToken cancellationToken = default)
    {
        await _context.Meals.AddAsync(meal, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Meal created: {MealId}", meal.Id);

        return meal;
    }

    public async Task<ErrorOr<Deleted>> DeleteMealAsync(Guid mealId, CancellationToken cancellationToken = default)
    {
        var meal = await _context.Meals.FirstOrDefaultAsync(m => m.Id == mealId, cancellationToken);
        if (meal is null)
            return Error.NotFound();

        _context.Meals.Remove(meal);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Meal deleted: {MealId}", mealId);

        return new Deleted();
    }

    public async Task<ErrorOr<IEnumerable<Meal>>> GetAllMealsAsync(CancellationToken cancellationToken = default)
    {
        var meals = await _context.Meals
            .Include(m => m.Restaurant)
            .Include(m => m.MealInOrders)
            .ToListAsync(cancellationToken);

        return meals;
    }

    public async Task<ErrorOr<Meal>> GetMealByIdAsync(Guid mealId, CancellationToken cancellationToken = default)
    {
        var meal = await _context.Meals
            .Include(m => m.Restaurant)
            .Include(m => m.MealInOrders)
            .FirstOrDefaultAsync(m => m.Id == mealId, cancellationToken);

        if (meal is null)
            return Error.NotFound();

        return meal;
    }

    public async Task<ErrorOr<Meal>> UpdateMealAsync(Meal meal, CancellationToken cancellationToken = default)
    {
        var existingMeal = await _context.Meals
            .Include(m => m.Restaurant)
            .Include(m => m.MealInOrders)
            .FirstOrDefaultAsync(m => m.Id == meal.Id, cancellationToken);

        if (existingMeal is null)
            return Error.NotFound();

        existingMeal.Name = meal.Name;
        existingMeal.Description = meal.Description;
        existingMeal.Price = meal.Price;
        existingMeal.RestaurantId = meal.RestaurantId;

        _context.Meals.Update(existingMeal);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Meal updated: {MealId}", existingMeal.Id);

        return existingMeal;
    }
}
