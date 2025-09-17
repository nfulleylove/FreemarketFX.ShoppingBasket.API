using FreemarketFX.ShoppingBasket.API.DataTransferObjects;
using FreemarketFX.ShoppingBasket.API.Models;

namespace FreemarketFX.ShoppingBasket.API.Mappers;

public static class BasketMapper
{
    public static BasketDto MapToDto(this Basket basket)
    {
        List<ProductDto> products =
            [.. basket.BasketProducts
            .Select(
                x =>
                new ProductDto
                {
                    Id = x.ProductId,
                    Name = x.Product?.Name ?? "",
                    Price = x.Product?.Price ?? 0,
                    Quantity = x.Quantity,
                    IsDiscounted = x.Product?.IsDiscounted ?? false,
                })];

        decimal subtotalExVat = GetSubtotalExcludingVat(basket.BasketProducts);
        decimal subtotalIncVat = RoundUp(subtotalExVat * 1.2M);
        decimal shipping = GetShippingCost(basket.Country);
        decimal total = RoundUp(subtotalIncVat + shipping);

        return new BasketDto
        {
            Id = basket.Id,
            Country = basket.Country,
            DiscountCode = basket.DiscountCode,
            Products = products,
            ShippingCost = shipping,
            SubtotalExcludingVat = subtotalExVat,
            SubtotalIncludingVat = subtotalIncVat,
            Total = total
        };
    }

    private static decimal GetSubtotalExcludingVat(List<BasketProduct> basketProducts)
        => RoundUp(basketProducts
            .Where(x => x.Product != null)
            .Sum(x => x.Product!.Price * x.Quantity));

    private static decimal GetShippingCost(string country)
    {
        return country?.ToUpper() switch
        {
            "UK" => 5M,
            "US" => 30M,
            "FR" => 20M,
            _ => 5M,
        };
    }

    // Never round down.
    private static decimal RoundUp(decimal value)
        => Math.Ceiling(value * 100) / 100;
}
