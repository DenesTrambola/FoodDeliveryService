using System.Text.Json.Serialization;

namespace FoodDeliveryService.Domain.Entities;

public class Meal
{
    public required Guid Id { get; set; }
    public required Guid RestaurantId { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public required string Description { get; set; }
    [JsonIgnore]
    public Restaurant Restaurant { get; set; } = null!;
    [JsonIgnore]
    public ICollection<MealInOrder> MealInOrders { get; set; } = [];
}
