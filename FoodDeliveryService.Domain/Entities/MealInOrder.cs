namespace FoodDeliveryService.Domain.Entities;

public class MealInOrder
{
    public required int MealId { get; set; }
    public required int OrderId { get; set; }
    public required int Quantity { get; set; }
    public Meal Meal { get; set; } = null!;
    public Order Order { get; set; } = null!;
}
