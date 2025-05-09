using ErrorOr;
using FoodDeliveryService.Domain.Entities;
using FoodDeliveryService.Domain.Enums;

namespace FoodDeliveryService.Application.Services;

public interface IOrderService
{
    Task<ErrorOr<IEnumerable<Order>>> GetAllOrdersAsync(CancellationToken cancellationToken = default);
    Task<ErrorOr<Order>> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<ErrorOr<OrderStatus>> TrackOrderStatus(Guid orderId, CancellationToken cancellationToken = default);
    Task<ErrorOr<Order>> CreateOrderAsync(Order order, IDictionary<Guid, int> mealInOrders, CancellationToken cancellationToken = default);
    Task<ErrorOr<Order>> UpdateOrderAsync(Order order, CancellationToken cancellationToken = default);
    Task<ErrorOr<Deleted>> DeleteOrderAsync(Guid orderId, CancellationToken cancellationToken = default);
}
