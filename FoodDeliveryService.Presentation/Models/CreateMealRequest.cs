namespace FoodDeliveryService.Presentation.Models;

public class CreateMealRequest
{
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public required string Description { get; set; }
    public required Guid RestaurantId { get; set; }
}
