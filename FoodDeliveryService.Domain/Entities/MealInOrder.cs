using System.Text.Json.Serialization;

namespace FoodDeliveryService.Domain.Entities;

public class MealInOrder
{
    public required Guid MealId { get; set; }
    public required Guid OrderId { get; set; }

    [JsonIgnore]
    public Meal Meal { get; set; } = null!;
    [JsonIgnore]
    public Order Order { get; set; } = null!;
    public required int Quantity { get; set; }
}
