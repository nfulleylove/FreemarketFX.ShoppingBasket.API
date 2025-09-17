using FreemarketFX.ShoppingBasket.API.Data.Interfaces;
using FreemarketFX.ShoppingBasket.API.DataTransferObjects;
using FreemarketFX.ShoppingBasket.API.Models;
using FreemarketFX.ShoppingBasket.API.Services;
using NSubstitute;

namespace FreemarketFX.ShoppingBasket.API.Tests.Services;
public class BasketServiceTests
{
    private readonly IBasketsRepository _repository;
    private readonly Guid _validId = Guid.NewGuid();
    private readonly Guid _invalidId = Guid.NewGuid();
    private readonly Guid _validProductId = Guid.NewGuid();
    private readonly Guid _addProductId = Guid.NewGuid();

    public BasketServiceTests()
    {
        _repository = Substitute.For<IBasketsRepository>();

        // Add a new basket
        _repository.AddBasketAsync().Returns(
            Task.FromResult<Basket>(
            new() { Id = Guid.NewGuid() }));

        // Get Basket with a valid ID
        _repository.GetBasketByIdAsync(_validId).Returns(
        Task.FromResult<Basket?>(new Basket
        {
            Id = _validId,
            Country = "US",
            DiscountCode = "10OFF",
            BasketProducts =
            [
                new BasketProduct
            {
                BasketId = _validId,
                ProductId = _validProductId,
                Product = new Product
                {
                    Id = _validProductId,
                    Name = "Table",
                    Price = 399.99M,
                    IsDiscounted = true
                },
                Quantity = 1
            }
            ]
        }));

        // Get a basket that doesn't exist
        _repository.GetBasketByIdAsync(_invalidId).Returns(
                    Task.FromResult<Basket?>(
                        null));

        // Add a product to an existing basket
        _repository.AddProductToBasketAsync(_validId, _addProductId).Returns(
            Task.FromResult<Basket>(new Basket
            {
                Id = _validId,
                BasketProducts =
                [
                    new BasketProduct
                {
                    BasketId = _validId,
                    ProductId = _addProductId,
                    Product = new Product
                    {
                        Id = _addProductId,
                        Name = "Chair",
                        Price = 44.99M,
                        IsDiscounted = true
                    },
                Quantity = 4
            }
        ]
            }));
    }

    [Fact]
    public async Task AddBasketAsync_ShouldReturnInsertedBasket()
    {
        // Arrange
        BasketService service = new(_repository);
        BasketDto basket;

        // Act
        basket = await service.AddBasketAsync();

        // Assert
        Assert.NotNull(basket);
        Assert.NotEqual(Guid.Empty, basket.Id);
        Assert.Empty(basket.Products);
    }

    [Fact]
    public async Task GetBasketByIdAsync_ShouldReturnBasket_ForValidId()
    {
        // Arrange
        BasketService service = new(_repository);
        BasketDto? actualBasket;
        Guid expectedId = _validId;
        Guid expectedProductId = _validProductId;
        int expectedProductsCount = 1;
        string expectedProductName = "Table";
        decimal expectedProductPrice = 399.99M;
        string expectedCountry = "US";
        string expectedDiscountCode = "10OFF";
        bool expectedIsDiscounted = true;
        int expectedQuantity = 1;

        // Act
        actualBasket = await service.GetBasketByIdAsync(expectedId);

        // Assert
        Assert.NotNull(actualBasket);
        Assert.Equal(expectedId, actualBasket.Id);
        Assert.Equal(expectedCountry, actualBasket.Country);
        Assert.Equal(expectedDiscountCode, actualBasket.DiscountCode);
        Assert.NotEmpty(actualBasket.Products);
        Assert.Equal(expectedProductsCount, actualBasket.Products.Count);
        Assert.Equal(expectedProductId, actualBasket.Products.First().Id);
        Assert.Equal(expectedProductName, actualBasket.Products.First().Name);
        Assert.Equal(expectedProductPrice, actualBasket.Products.First().Price);
        Assert.Equal(expectedIsDiscounted, actualBasket.Products.First().IsDiscounted);
        Assert.Equal(expectedQuantity, actualBasket.Products.First().Quantity);
    }

    [Fact]
    public async Task GetBasketByIdAsync_ShouldReturnNull_ForInvalidId()
    {
        // Arrange
        BasketService service = new(_repository);
        BasketDto? actualBasket;
        Guid expectedId = _invalidId;

        // Act
        actualBasket = await service.GetBasketByIdAsync(expectedId);

        // Assert
        Assert.Null(actualBasket);
    }

    [Fact]
    public async Task AddProductToBasketAsync_ShouldReturnBasketWithProduct_ForValidIds()
    {
        // Arrange
        BasketService service = new(_repository);
        BasketDto? actualBasket;
        Guid expectedId = _validId;
        Guid expectedProductId = _addProductId;
        int expectedProductsCount = 1;
        string expectedProductName = "Chair";
        decimal expectedProductPrice = 44.99M;
        bool expectedIsDiscounted = true;
        int expectedQuantity = 4;

        // Act
        actualBasket = await service.AddProductToBasketAsync(expectedId, expectedProductId);

        // Assert
        Assert.NotNull(actualBasket);
        Assert.Equal(expectedId, actualBasket.Id);
        Assert.NotEmpty(actualBasket.Products);
        Assert.Equal(expectedProductsCount, actualBasket.Products.Count);
        Assert.Equal(expectedProductId, actualBasket.Products.First().Id);
        Assert.Equal(expectedProductName, actualBasket.Products.First().Name);
        Assert.Equal(expectedProductPrice, actualBasket.Products.First().Price);
        Assert.Equal(expectedIsDiscounted, actualBasket.Products.First().IsDiscounted);
        Assert.Equal(expectedQuantity, actualBasket.Products.First().Quantity);
    }
}
