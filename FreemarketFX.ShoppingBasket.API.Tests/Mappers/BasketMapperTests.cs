using FreemarketFX.ShoppingBasket.API.DataTransferObjects;
using FreemarketFX.ShoppingBasket.API.Mappers;
using FreemarketFX.ShoppingBasket.API.Models;

namespace FreemarketFX.ShoppingBasket.API.Tests.Mappers;
public class BasketMapperTests
{
    [Fact]
    public void MapToDto_ShouldMapAllProperties_FromBasketToBasketDto()
    {
        // Arrange
        Guid productId = Guid.NewGuid();
        BasketDto? actualDto;

        Basket basket = new()
        {
            Id = Guid.NewGuid(),
            Country = "UK",
            DiscountCode = "SAVE10",
            BasketProducts =
            [
                new BasketProduct
                {
                    ProductId = productId,
                    Quantity = 2,
                    Product = new Product
                    {
                        Id = productId,
                        Name = "Desk Lamp",
                        Price = 29.99M,
                        IsDiscounted = true
                    }
                }
            ]
        };

        // Act
        actualDto = basket.MapToDto();

        // Assert
        Assert.NotNull(actualDto);
        Assert.Equal(basket.Id, actualDto.Id);
        Assert.Equal(basket.Country, actualDto.Country);
        Assert.Equal(basket.DiscountCode, actualDto.DiscountCode);
        Assert.Single(actualDto.Products);

        BasketProduct product = basket.BasketProducts.First();
        ProductDto productDto = actualDto.Products.First();

        Assert.Equal(product.Product!.Id, productDto.Id);
        Assert.Equal(product.Product!.Name, productDto.Name);
        Assert.Equal(product.Product!.Price, productDto.Price);
        Assert.Equal(product.Quantity, productDto.Quantity);
        Assert.True(productDto.IsDiscounted);
    }

    [Fact]
    public void MapToDto_ShouldMapNullProperties()
    {
        // Arrange
        BasketDto? actualDto;

        var basket = new Basket
        {
            Id = Guid.NewGuid(),
            Country = null!,
            DiscountCode = null,
            BasketProducts =
            [
                new() {
                    ProductId = Guid.NewGuid(),
                    Quantity = 1,
                    Product = null
                }
            ]
        };

        // Act
        actualDto = basket.MapToDto();

        // Assert
        Assert.Equal(basket.Country, actualDto.Country);
        Assert.Equal(basket.DiscountCode, actualDto.DiscountCode);

        ProductDto productDto = actualDto.Products.First();
        Assert.Equal("", productDto.Name);
        Assert.Equal(0, productDto.Price);
        Assert.False(productDto.IsDiscounted);
    }
}