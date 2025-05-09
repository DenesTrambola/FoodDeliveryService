using ErrorOr;
using FoodDeliveryService.Domain.Entities;

namespace FoodDeliveryService.Application.Services;

public interface IRestaurantService
{
    Task<ErrorOr<IEnumerable<Restaurant>>> GetAllRestaurantsAsync(CancellationToken cancellationToken = default);
    Task<ErrorOr<Restaurant>> GetRestaurantByIdAsync(Guid restaurantId, CancellationToken cancellationToken = default);
    Task<ErrorOr<Restaurant>> CreateRestaurantAsync(Restaurant restaurant, CancellationToken cancellationToken = default);
    Task<ErrorOr<Restaurant>> UpdateRestaurantAsync(Restaurant restaurant, CancellationToken cancellationToken = default);
    Task<ErrorOr<Deleted>> DeleteRestaurantAsync(Guid restaurantId, CancellationToken cancellationToken = default);
}
