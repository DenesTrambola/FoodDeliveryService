using ErrorOr;
using FoodDeliveryService.Application.Services;
using FoodDeliveryService.Domain.Entities;
using FoodDeliveryService.Domain.Enums;
using FoodDeliveryService.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FoodDeliveryService.Infrastructure.Persistence.Services;

public class OrderService(FoodDeliveryDbContext context, ILogger<OrderService> logger) : IOrderService
{
    private readonly FoodDeliveryDbContext _context = context;
    private readonly ILogger<OrderService> _logger = logger;

    public async Task<ErrorOr<Order>> CreateOrderAsync(Order order, IDictionary<Guid, int> mealIdQuantities, CancellationToken cancellationToken = default)
    {
        if (!mealIdQuantities.Any())
            return Error.Validation("At least one meal must be included in the order");

        foreach (var mealIdQuantity in mealIdQuantities)
        {
            var meal = await context.Meals.FirstOrDefaultAsync(d => d.Id == mealIdQuantity.Key, cancellationToken);
            if (meal is null)
                return Error.NotFound($"Meal with ID {mealIdQuantity.Key} not found!");

            order.MealInOrders.Add(new MealInOrder
            {
                MealId = mealIdQuantity.Key,
                OrderId = order.Id,
                Quantity = mealIdQuantity.Value
            });

            order.TotalPrice += meal.Price * mealIdQuantity.Value;
        }

        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Order created: {OrderId}", order.Id);

        return order;
    }

    public async Task<ErrorOr<Deleted>> DeleteOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order is null)
            return Error.NotFound();

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Order deleted: {OrderId}", orderId);

        return new Deleted();
    }

    public async Task<ErrorOr<IEnumerable<Order>>> GetAllOrdersAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _context.Orders
            .Include(o => o.Restaurant)
            .Include(o => o.MealInOrders)
            .ToListAsync(cancellationToken);

        return orders;
    }

    public async Task<ErrorOr<Order>> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders
            .Include(o => o.Restaurant)
            .Include(o => o.MealInOrders)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order is null)
            return Error.NotFound();

        return order;
    }

    public async Task<ErrorOr<OrderStatus>> TrackOrderStatus(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order is null)
            return Error.NotFound();

        return order.Status;
    }

    public async Task<ErrorOr<Order>> UpdateOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
        var existingOrder = await _context.Orders
            .Include(o => o.Restaurant)
            .Include(o => o.MealInOrders)
            .FirstOrDefaultAsync(o => o.Id == order.Id, cancellationToken);

        if (existingOrder is null)
            return Error.NotFound();

        existingOrder.RestaurantId = order.RestaurantId;
        existingOrder.MealInOrders = order.MealInOrders;
        existingOrder.TotalPrice = order.TotalPrice;
        existingOrder.Status = order.Status;

        _context.Orders.Update(existingOrder);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Order updated: {OrderId}", order.Id);

        return existingOrder;
    }
}
