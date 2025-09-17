using FreemarketFX.ShoppingBasket.API.Models;

namespace FreemarketFX.ShoppingBasket.API.Data.Interfaces;

public interface IBasketsRepository
{
    Task<Basket> AddBasketAsync();
    Task<Basket?> GetBasketByIdAsync(Guid id);
    Task<Basket> AddProductToBasketAsync(Guid basketId, Guid productId);
    Task<Basket> AddProductsToBasketAsync(Guid basketId, IEnumerable<Guid> productIds);
    Task RemoveProductAsync(Guid basketId, Guid productId);
    Task AddDiscountCodeAsync(string code);
    Task SetCountryAsync(Guid countryId);
}
