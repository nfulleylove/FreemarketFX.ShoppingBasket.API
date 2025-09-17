using FreemarketFX.ShoppingBasket.API.DataTransferObjects;
using FreemarketFX.ShoppingBasket.API.Models;
using FreemarketFX.ShoppingBasket.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FreemarketFX.ShoppingBasket.Controllers;

[ApiController]
[Route("[controller]")]
public class BasketsController(
    IBasketService basketService,
    ILogger<BasketsController> logger) : ControllerBase
{
    private readonly IBasketService _basketService = basketService;
    private readonly ILogger<BasketsController> _logger = logger;

    [HttpPost(Name = "AddBasket")]
    [ProducesResponseType<Basket>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BasketDto>> InsertBasket()
    {
        try
        {
            BasketDto basket = await _basketService.AddBasketAsync();

            return CreatedAtAction(nameof(InsertBasket), basket);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding basket");

            return Problem("There was an error adding the basket.");
        }
    }

    [HttpGet("{id}", Name = "GetBasketById")]
    [ProducesResponseType<BasketDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BasketDto>> GetBasketById(Guid id)
    {
        try
        {
            BasketDto? basket = await _basketService.GetBasketByIdAsync(id);

            if (basket == null)
                return NotFound();

            return Ok(basket);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving basket.");

            return Problem("There was an error retrieving the basket.");
        }
    }
}
