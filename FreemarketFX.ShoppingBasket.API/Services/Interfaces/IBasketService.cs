using FreemarketFX.ShoppingBasket.API.DataTransferObjects;

namespace FreemarketFX.ShoppingBasket.API.Services.Interfaces;
public interface IBasketService
{
    Task<BasketDto> AddBasketAsync();
    Task<BasketDto?> GetBasketByIdAsync(Guid id);
    Task<BasketDto> AddProductToBasketAsync(Guid basketId, Guid productId);
    Task<BasketDto> AddProductsToBasketAsync(Guid basketId, IEnumerable<Guid> productIds);
    Task RemoveProductAsync(Guid basketId, Guid productId);
    Task AddDiscountCodeAsync(string code);
    Task SetCountryAsync(Guid countryId);
}