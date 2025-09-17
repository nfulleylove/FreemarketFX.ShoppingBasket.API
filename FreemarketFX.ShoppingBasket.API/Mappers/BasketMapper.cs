using FreemarketFX.ShoppingBasket.API.DataTransferObjects;
using FreemarketFX.ShoppingBasket.API.Models;

namespace FreemarketFX.ShoppingBasket.API.Mappers;

public static class BasketMapper
{
    public static BasketDto MapToDto(this Basket basket)
    {
        return new BasketDto
        {
            Id = basket.Id,
            Country = basket.Country,
            DiscountCode = basket.DiscountCode,
            Products = [.. basket.BasketProducts
            .Select(
                x
                => new ProductDto
                {
                    Id = x.ProductId,
                    Name = x.Product?.Name ?? "",
                    Price = x.Product?.Price ?? 0,
                    Quantity = x.Quantity,
                    IsDiscounted = x.Product?.IsDiscounted ?? false,
                })],
        };
    }
}
