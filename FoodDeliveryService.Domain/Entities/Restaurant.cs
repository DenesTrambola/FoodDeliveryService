namespace FoodDeliveryService.Domain.Entities;

public class Restaurant
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required string Description { get; set; }
    public ICollection<Meal> Meals { get; set; } = [];
    public ICollection<Order> Orders { get; set; } = [];
}
