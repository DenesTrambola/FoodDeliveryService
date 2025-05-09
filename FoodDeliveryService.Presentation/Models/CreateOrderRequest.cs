using FoodDeliveryService.Domain.Enums;

namespace FoodDeliveryService.Presentation.Models;

public class CreateOrderRequest
{
    public required Guid RestaurantId { get; set; }
    public required OrderStatus Status { get; set; }
    public required Dictionary<Guid, int> MealIdQuantities { get; set; } = [];
}
