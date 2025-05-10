using FoodDeliveryService.Application.Services;
using FoodDeliveryService.Domain.Entities;
using FoodDeliveryService.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryService.Presentation.Controllers;

[Route("orders")]
public class OrderController(IOrderService service) : ApiController
{
    private readonly IOrderService _service = service;

    /// <summary>
    /// Get all orders.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of all orders.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Order>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var orders = await _service.GetAllOrdersAsync(cancellationToken);

        if (orders.IsError)
            return BadRequest(orders.Errors);
        else if (orders.Value is null || orders.Value.Count() == 0)
            return NotFound("No orders found.");

        return Ok(orders.Value);
    }

    /// <summary>
    /// Get an order by ID.
    /// </summary>
    /// <param name="orderId">Order identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Order details for the specified ID.</returns>
    [HttpGet("{orderId:guid}")]
    [ProducesResponseType(typeof(Order), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetById(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await _service.GetOrderByIdAsync(orderId, cancellationToken);

        if (order.IsError)
            return BadRequest(order.Errors);
        else if (order.Value is null)
            return NotFound($"Order with ID {orderId} not found.");

        return Ok(order.Value);
    }

    /// <summary>
    /// Track the status of an order by ID.
    /// </summary>
    /// <param name="orderId">Order identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Current status of the specified order.</returns>
    [HttpGet("track/{orderId:guid}")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> TrackStatus(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await _service.TrackOrderStatus(orderId, cancellationToken);

        if (order.IsError)
            return BadRequest(order.Errors);

        return Ok(order.Value.ToString());
    }

    /// <summary>
    /// Create a new order.
    /// </summary>
    /// <param name="request">Order creation details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created order.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Order), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
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

    /// <summary>
    /// Update an existing order.
    /// </summary>
    /// <param name="orderId">Order identifier.</param>
    /// <param name="request">Updated order details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated order.</returns>
    [HttpPut("{orderId:guid}")]
    [ProducesResponseType(typeof(Order), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Update(Guid orderId, [FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
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

    /// <summary>
    /// Delete an order by ID.
    /// </summary>
    /// <param name="orderId">Order identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Confirmation of order deletion.</returns>
    [HttpDelete("{orderId:guid}")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Delete(Guid orderId, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteOrderAsync(orderId, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok("Order deleted successfully!");
    }
}
