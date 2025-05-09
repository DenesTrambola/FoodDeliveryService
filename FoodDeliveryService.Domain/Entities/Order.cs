using FoodDeliveryService.Domain.Enums;
using System.Text.Json.Serialization;

namespace FoodDeliveryService.Domain.Entities;

public class Order
{
    public required Guid Id { get; set; }
    public required Guid RestaurantId { get; set; }
    public required decimal TotalPrice { get; set; }
    public required OrderStatus Status { get; set; }
    [JsonIgnore]
    public Restaurant Restaurant { get; set; } = null!;
    [JsonIgnore]
    public ICollection<MealInOrder> MealInOrders { get; set; } = [];
}
