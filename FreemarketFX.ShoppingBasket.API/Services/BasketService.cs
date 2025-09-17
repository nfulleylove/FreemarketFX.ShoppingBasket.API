using FreemarketFX.ShoppingBasket.API.Data.Interfaces;
using FreemarketFX.ShoppingBasket.API.DataTransferObjects;
using FreemarketFX.ShoppingBasket.API.Mappers;
using FreemarketFX.ShoppingBasket.API.Models;
using FreemarketFX.ShoppingBasket.API.Services.Interfaces;

namespace FreemarketFX.ShoppingBasket.API.Services;

public class BasketService(IBasketsRepository repository) : IBasketService
{
    private readonly IBasketsRepository _repository = repository;

    public async Task<BasketDto> AddBasketAsync()
    {
        Basket basket = await _repository.AddBasketAsync();

        return basket.MapToDto();
    }

    public async Task<BasketDto?> GetBasketByIdAsync(Guid id)
    {
        Basket? basket = await _repository.GetBasketByIdAsync(id);

        return basket?.MapToDto();
    }

    public async Task<BasketDto> AddProductToBasketAsync(Guid basketId, Guid productId)
    {
        Basket basket = await _repository.AddProductToBasketAsync(basketId, productId);

        return basket.MapToDto();
    }

    public Task<BasketDto> AddProductsToBasketAsync(Guid basketId, IEnumerable<Guid> productIds)
    {
        throw new NotImplementedException();
    }

    public Task RemoveProductAsync(Guid basketId, Guid productId)
    {
        throw new NotImplementedException();
    }

    public Task AddDiscountCodeAsync(string code)
    {
        throw new NotImplementedException();
    }

    public Task SetCountryAsync(Guid countryId)
    {
        throw new NotImplementedException();
    }
}
