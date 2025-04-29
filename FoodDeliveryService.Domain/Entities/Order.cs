using FoodDeliveryService.Domain.Enums;

namespace FoodDeliveryService.Domain.Entities;

public class Order
{
    public required int Id { get; set; }
    public required int RestaurantId { get; set; }
    public required decimal TotalPrice { get; set; }
    public required DateTime OrderDate { get; set; }
    public required OrderStatus Status { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
    public ICollection<MealInOrder> MealInOrders { get; set; } = [];
}
