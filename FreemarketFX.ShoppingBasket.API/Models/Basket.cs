using System.Text.Json.Serialization;

namespace FreemarketFX.ShoppingBasket.API.Models;

public class Basket
{
    public Guid Id { get; set; }
    public string? DiscountCode { get; set; }
    public string Country { get; set; } = "UK";

    [JsonIgnore]
    public List<BasketProduct> BasketProducts { get; set; } = [];
}
