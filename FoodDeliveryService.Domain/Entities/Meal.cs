namespace FoodDeliveryService.Domain.Entities;

public class Meal
{
    public required int Id { get; set; }
    public required int RestaurantId { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public required string Description { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
    public ICollection<MealInOrder> MealInOrders { get; set; } = [];
}
