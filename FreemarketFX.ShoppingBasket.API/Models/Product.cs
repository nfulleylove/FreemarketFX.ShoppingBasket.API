using System.Text.Json.Serialization;

namespace FreemarketFX.ShoppingBasket.API.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public bool IsDiscounted { get; set; } = false;


    [JsonIgnore]
    public List<BasketProduct> BasketProducts { get; set; } = [];
}
