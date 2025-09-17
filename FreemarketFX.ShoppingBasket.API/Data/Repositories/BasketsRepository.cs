using FreemarketFX.ShoppingBasket.API.Data.Interfaces;
using FreemarketFX.ShoppingBasket.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FreemarketFX.ShoppingBasket.API.Data.Repositories;

public class BasketsRepository(ShoppingDbContext context) : IBasketsRepository
{
    private readonly ShoppingDbContext _context = context;

    public async Task<Basket> AddBasketAsync()
    {
        EntityEntry<Basket> basket = _context.Baskets.Add(new Basket());

        await _context.SaveChangesAsync();

        return basket.Entity;
    }

    public async Task<Basket?> GetBasketByIdAsync(Guid id)
    {
        Basket? basket = await _context.Baskets
            .Include(x => x.BasketProducts)
            .ThenInclude(x => x.Product)
            .SingleOrDefaultAsync(
            x =>
            x.Id == id);

        return basket;
    }

    public Task<Basket> AddProductToBasketAsync(Guid basketId, Guid productId)
    {
        throw new NotImplementedException();
    }

    public Task<Basket> AddProductsToBasketAsync(Guid basketId, IEnumerable<Guid> productIds)
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
