using System.Text.Json.Serialization;

namespace FoodDeliveryService.Domain.Entities;

public class Restaurant
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required string Description { get; set; }
    [JsonIgnore]
    public ICollection<Meal> Meals { get; set; } = [];
    [JsonIgnore]
    public ICollection<Order> Orders { get; set; } = [];
}
