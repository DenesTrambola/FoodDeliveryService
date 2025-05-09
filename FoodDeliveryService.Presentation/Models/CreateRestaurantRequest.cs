namespace FoodDeliveryService.Presentation.Models;

public class CreateRestaurantRequest
{
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required string Description { get; set; }
}
