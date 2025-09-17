using FreemarketFX.ShoppingBasket.API.DataTransferObjects;
using FreemarketFX.ShoppingBasket.API.Models;
using FreemarketFX.ShoppingBasket.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FreemarketFX.ShoppingBasket.API.Controllers;

[ApiController]
[Route("Baskets/{id}/Products")]
public class BasketProductsController(
    IBasketService basketService,
    ILogger<BasketProductsController> logger) : ControllerBase
{
    private readonly IBasketService _basketService = basketService;
    private readonly ILogger<BasketProductsController> _logger = logger;

    /// <summary>
    /// Adds a product to a basket.
    /// </summary>
    /// <param name="id" example="A1D8D4B0-551D-4A26-A417-5EE3FFE3A4E2">The ID of the basket.</param>
    /// <param name="productId" example="43AA1547-F36C-480A-A680-2917BAAD4DB1">The ID of the product.</param>
    /// <returns></returns>
    [HttpPost(Name = "AddProduct")]
    [ProducesResponseType<Basket>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BasketDto>> AddProduct(
        Guid id,
        Guid productId)
    {
        try
        {
            BasketDto basket = await _basketService.AddProductToBasketAsync(id, productId);

            return CreatedAtRoute("GetBasketById", new { controller = "Baskets", id }, basket);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding product to basket.");

            return Problem("There was an error adding the product to the basket.");
        }
    }
}
