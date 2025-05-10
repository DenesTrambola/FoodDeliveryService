using FoodDeliveryService.Application.Services;
using FoodDeliveryService.Domain.Entities;
using FoodDeliveryService.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryService.Presentation.Controllers;

[Route("restaurants")]
public class RestaurantController(IRestaurantService service) : ApiController
{
    IRestaurantService _service = service;

    /// <summary>
    /// Get all restaurants.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of all restaurants.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Restaurant>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var restaurants = await _service.GetAllRestaurantsAsync();
        if (restaurants.IsError)
            return BadRequest(restaurants.Errors);
        else if (restaurants.Value is null || restaurants.Value.Count() == 0)
            return NotFound("No restaurants found.");

        return Ok(restaurants.Value);
    }

    /// <summary>
    /// Get a restaurant by ID.
    /// </summary>
    /// <param name="restaurantId">Restaurant identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Restaurant details for the specified ID.</returns>
    [HttpGet("{restaurantId:guid}")]
    [ProducesResponseType(typeof(Restaurant), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetById(Guid restaurantId, CancellationToken cancellationToken)
    {
        var restaurant = await _service.GetRestaurantByIdAsync(restaurantId);
        if (restaurant.IsError)
            return BadRequest(restaurant.Errors);
        else if (restaurant.Value is null)
            return NotFound($"Restaurant with ID {restaurantId} not found.");

        return Ok(restaurant.Value);
    }

    /// <summary>
    /// Create a new restaurant.
    /// </summary>
    /// <param name="request">Restaurant creation details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created restaurant.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Restaurant), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Create([FromBody] CreateRestaurantRequest request, CancellationToken cancellationToken)
    {
        Restaurant restaurant = new()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Description = request.Description,
        };

        var result = await _service.CreateRestaurantAsync(restaurant);
        if (result.IsError)
            return BadRequest(result.Errors);
        else if (result.Value is null)
            return NotFound("Failed to create restaurant.");

        return Ok(result.Value);
    }

    /// <summary>
    /// Update an existing restaurant.
    /// </summary>
    /// <param name="restaurantId">Restaurant identifier.</param>
    /// <param name="request">Updated restaurant details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated restaurant.</returns>
    [HttpPut("{restaurantId:guid}")]
    [ProducesResponseType(typeof(Restaurant), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Update(Guid restaurantId, [FromBody] CreateRestaurantRequest request, CancellationToken cancellationToken)
    {
        Restaurant restaurant = new()
        {
            Id = restaurantId,
            Name = request.Name,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Description = request.Description,
        };

        var result = await _service.UpdateRestaurantAsync(restaurant);

        if (result.IsError)
            return BadRequest(result.Errors);
        else if (result.Value is null)
            return NotFound($"Restaurant with ID {restaurantId} not found.");

        return Ok(result.Value);
    }

    /// <summary>
    /// Delete a restaurant by ID.
    /// </summary>
    /// <param name="restaurantId">Restaurant identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Confirmation of restaurant deletion.</returns>
    [HttpDelete("{restaurantId:guid}")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Delete(Guid restaurantId, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteRestaurantAsync(restaurantId);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok("Restaurant deleted successfully!");
    }
}
