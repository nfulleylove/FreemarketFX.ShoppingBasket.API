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

    public async Task<Basket> AddProductToBasketAsync(Guid basketId, Guid productId)
    {
        Basket basket = await GetBasketByIdAsync(basketId)
            ?? throw new KeyNotFoundException($"Basket not found in database with ID: {productId}");

        Product product = await _context.Products
            .SingleOrDefaultAsync(
            x =>
            x.Id == productId)
            ?? throw new KeyNotFoundException($"Product not found in database with ID: {productId}");

        // Check basket for existing product
        BasketProduct? basketProduct = basket.BasketProducts
            .SingleOrDefault(
            x =>
            x.BasketId == basketId &&
            x.ProductId == productId);

        // Add product or increment quantity
        if (basketProduct != null)
            basketProduct.Quantity += 1;
        else
            basket.BasketProducts.Add(new BasketProduct { BasketId = basketId, ProductId = productId });


        await _context.SaveChangesAsync();

        return basket;
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
