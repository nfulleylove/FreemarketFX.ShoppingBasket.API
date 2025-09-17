namespace FreemarketFX.ShoppingBasket.API.DataTransferObjects;

public record ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; } = 0;
    public int Quantity { get; set; } = 1;
    public bool IsDiscounted { get; set; } = false;
}
