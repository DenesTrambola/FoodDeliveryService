using FoodDeliveryService.Application.Services;
using FoodDeliveryService.Domain.Entities;
using FoodDeliveryService.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryService.Presentation.Controllers;

[Route("meals")]
public class MealController(IMealService service) : ApiController
{
    IMealService _service = service;

    /// <summary>
    /// Retrieves all meals.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A list of all available meals.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Meal>), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var meals = await _service.GetAllMealsAsync();

        if (meals.IsError)
            return BadRequest(meals.Errors);
        else if (meals.Value is null || meals.Value.Count() == 0)
            return NotFound("No meals found.");

        return Ok(meals.Value);
    }

    /// <summary>
    /// Retrieves a specific meal by its ID.
    /// </summary>
    /// <param name="mealId">Unique identifier of the meal.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>The requested meal if found.</returns>
    [HttpGet("{mealId:guid}")]
    [ProducesResponseType(typeof(Meal), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetById(Guid mealId, CancellationToken cancellationToken)
    {
        var meal = await _service.GetMealByIdAsync(mealId);

        if (meal.IsError)
            return BadRequest(meal.Errors);
        else if (meal.Value is null)
            return NotFound($"Meal with ID {mealId} not found.");

        return Ok(meal.Value);
    }

    /// <summary>
    /// Creates a new meal.
    /// </summary>
    /// <param name="meal">Details of the meal to be created.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>The newly created meal.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Meal), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]

    public async Task<IActionResult> Create([FromBody] CreateMealRequest meal, CancellationToken cancellationToken)
    {
        var newMeal = new Meal
        {
            Id = Guid.NewGuid(),
            RestaurantId = meal.RestaurantId,
            Name = meal.Name,
            Price = meal.Price,
            Description = meal.Description
        };

        var result = await _service.CreateMealAsync(newMeal);
        if (result.IsError)
            return BadRequest(result.Errors);
        else if (result.Value is null)
            return NotFound("Failed to create meal.");

        return Ok(result.Value);
    }

    /// <summary>
    /// Updates an existing meal by ID.
    /// </summary>
    /// <param name="mealId">Unique identifier of the meal to update.</param>
    /// <param name="meal">Updated meal details.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>The updated meal.</returns>
    [HttpPut("{mealId:guid}")]
    [ProducesResponseType(typeof(Meal), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Update(Guid mealId, [FromBody] CreateMealRequest meal, CancellationToken cancellationToken)
    {
        var updatedMeal = new Meal
        {
            Id = mealId,
            RestaurantId = meal.RestaurantId,
            Name = meal.Name,
            Price = meal.Price,
            Description = meal.Description
        };

        var result = await _service.UpdateMealAsync(updatedMeal);
        if (result.IsError)
            return BadRequest(result.Errors);
        else if (result.Value is null)
            return NotFound("Failed to update meal.");

        return Ok(result.Value);
    }

    /// <summary>
    /// Deletes a meal by ID.
    /// </summary>
    /// <param name="mealId">Unique identifier of the meal to delete.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A confirmation message upon successful deletion.</returns>
    [HttpDelete("{mealId:guid}")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Delete(Guid mealId, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteMealAsync(mealId);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok("Meal deleted successfully!");
    }
}
