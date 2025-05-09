using ErrorOr;
using FoodDeliveryService.Domain.Entities;

namespace FoodDeliveryService.Application.Services;

public interface IMealService
{
    Task<ErrorOr<IEnumerable<Meal>>> GetAllMealsAsync(CancellationToken cancellationToken = default);
    Task<ErrorOr<Meal>> GetMealByIdAsync(Guid mealId, CancellationToken cancellationToken = default);
    Task<ErrorOr<Meal>> CreateMealAsync(Meal meal, CancellationToken cancellationToken = default);
    Task<ErrorOr<Meal>> UpdateMealAsync(Meal meal, CancellationToken cancellationToken = default);
    Task<ErrorOr<Deleted>> DeleteMealAsync(Guid mealId, CancellationToken cancellationToken = default);
}
