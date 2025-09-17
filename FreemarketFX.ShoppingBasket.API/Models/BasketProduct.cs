namespace FreemarketFX.ShoppingBasket.API.Models;

public class BasketProduct
{
    public Guid BasketId { get; set; }
    public Basket? Basket { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

    public int Quantity { get; set; } = 1;
}