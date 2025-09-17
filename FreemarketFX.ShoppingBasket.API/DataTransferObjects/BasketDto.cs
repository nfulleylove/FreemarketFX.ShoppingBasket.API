namespace FreemarketFX.ShoppingBasket.API.DataTransferObjects;

public record BasketDto
{
    public Guid Id { get; set; }
    public List<ProductDto> Products { get; set; } = [];
    public string? DiscountCode { get; set; }
    public string Country { get; set; } = string.Empty;
    public decimal ShippingCost { get; set; }
    public decimal SubtotalExcludingVat { get; set; }
    public decimal SubtotalIncludingVat { get; set; }
    public decimal Total { get; set; }
}
