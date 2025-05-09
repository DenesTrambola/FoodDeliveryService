using FoodDeliveryService.Application.Services;
using FoodDeliveryService.Domain.Entities;
using FoodDeliveryService.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryService.Presentation.Controllers;

[Route("orders")]
public class OrderController(IOrderService service) : ApiController
{
    private readonly IOrderService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetOrders(CancellationToken cancellationToken)
    {
        var orders = await _service.GetAllOrdersAsync(cancellationToken);

        if (orders.IsError)
            return BadRequest(orders.Errors);
        else if (orders.Value is null || orders.Value.Count() == 0)
            return NotFound("No orders found.");

        return Ok(orders.Value);
    }

    [HttpGet("{orderId:guid}")]
    public async Task<IActionResult> GetOrderById(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await _service.GetOrderByIdAsync(orderId, cancellationToken);

        if (order.IsError)
            return BadRequest(order.Errors);
        else if (order.Value is null)
            return NotFound($"Order with ID {orderId} not found.");

        return Ok(order.Value);
    }

    [HttpGet("track/{orderId:guid}")]
    public async Task<IActionResult> TrackOrderStatus(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await _service.TrackOrderStatus(orderId, cancellationToken);

        if (order.IsError)
            return BadRequest(order.Errors);

        return Ok(order.Value.ToString());
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            RestaurantId = request.RestaurantId,
            TotalPrice = 0,
            Status = request.Status
        };

        var result = await _service.CreateOrderAsync(order, request.MealIdQuantities, cancellationToken);
        if (result.IsError)
            return BadRequest(result.Errors);
        else if (result.Value is null)
            return NotFound("Failed to create order.");

        return Ok(result.Value);
    }

    [HttpPut("{orderId:guid}")]
    public async Task<IActionResult> UpdateOrder(Guid orderId, [FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Id = orderId,
            RestaurantId = request.RestaurantId,
            TotalPrice = 0,
            Status = request.Status
        };

        var result = await _service.UpdateOrderAsync(order, cancellationToken);
        if (result.IsError)
            return BadRequest(result.Errors);
        else if (result.Value is null)
            return NotFound($"Failed to update order with ID {orderId}.");

        return Ok(result.Value);
    }

    [HttpDelete("{orderId:guid}")]
    public async Task<IActionResult> DeleteOrder(Guid orderId, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteOrderAsync(orderId, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok("Order deleted successfully!");
    }
}
