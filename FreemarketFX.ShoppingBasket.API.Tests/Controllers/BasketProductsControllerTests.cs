using FreemarketFX.ShoppingBasket.API.Controllers;
using FreemarketFX.ShoppingBasket.API.DataTransferObjects;
using FreemarketFX.ShoppingBasket.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace FreemarketFX.ShoppingBasket.API.Tests.Controllers;
public class BasketProductsControllerTests
{
    private readonly ILogger<BasketProductsController> _logger;
    private readonly IBasketService _service;
    private readonly Guid _validBasketId = Guid.NewGuid();
    private readonly Guid _validProductId = Guid.NewGuid();
    private readonly Guid _invalidBasketId = Guid.NewGuid();
    private readonly Guid _invalidProductId = Guid.NewGuid();
    private readonly Guid _exceptionId = Guid.NewGuid();

    public BasketProductsControllerTests()
    {
        _logger = Substitute.For<ILogger<BasketProductsController>>();
        _service = Substitute.For<IBasketService>();

        _service.AddProductToBasketAsync(_validBasketId, _validProductId).Returns(
            Task.FromResult<BasketDto>(new BasketDto
            {
                Id = _validBasketId,
                Country = "UK",
                Products =
                [
                    new ProductDto
                {
                    Id = _validProductId,
                        Name = "Chair",
                        Price = 44.99M,
                        IsDiscounted = true,
                        Quantity = 4
                    }
                ]
            }));

        _service.AddProductToBasketAsync(_invalidBasketId, _validProductId)
            .Throws(new KeyNotFoundException($"Basket not found in database with ID: {_invalidBasketId}"));

        _service.AddProductToBasketAsync(_validBasketId, _invalidProductId)
            .Throws(new KeyNotFoundException($"Product not found in database with ID: {_invalidProductId}"));

        _service.AddProductToBasketAsync(_exceptionId, _exceptionId).Throws(
            new Exception("A database error has occurred."));
    }

    [Fact]
    public async Task AddProduct_ShouldReturnBasketWithAddedProduct()
    {
        // Arrange
        BasketProductsController controller = new(_service, _logger);
        ActionResult<BasketDto>? result;

        // Act
        result = await controller.AddProduct(_validBasketId, _validProductId);

        // Assert
        var createdResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
        var actualBasket = Assert.IsType<BasketDto>(createdResult.Value);

        Assert.NotNull(actualBasket);
        Assert.NotEqual(Guid.Empty, actualBasket.Id);
        Assert.NotEmpty(actualBasket.Products);
    }

    [Fact]
    public async Task AddProduct_ShouldReturnNotFound_ForInvalidBasketId()
    {
        // Arrange
        BasketProductsController controller = new(_service, _logger);
        ActionResult<BasketDto>? result;

        // Act
        result = await controller.AddProduct(_invalidBasketId, _validProductId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task AddProduct_ShouldReturnNotFound_ForInvalidProductId()
    {
        // Arrange
        BasketProductsController controller = new(_service, _logger);
        ActionResult<BasketDto>? result;

        // Act
        result = await controller.AddProduct(_validBasketId, _invalidProductId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task AddProduct_ShouldReturnProblem_WhenErrorOccurs()
    {
        // Arrange
        BasketProductsController controller = new(_service, _logger);
        ActionResult<BasketDto>? result;
        string expectedErrorMessage = "There was an error adding the product to the basket.";

        // Act
        result = await controller.AddProduct(_exceptionId, _exceptionId);

        // Assert
        var problemResult = Assert.IsType<ObjectResult>(result.Result);
        var problemDetails = Assert.IsType<ProblemDetails>(problemResult.Value);

        Assert.Equal(expectedErrorMessage, problemDetails.Detail);
    }
}
