using FreemarketFX.ShoppingBasket.API.DataTransferObjects;
using FreemarketFX.ShoppingBasket.API.Services.Interfaces;
using FreemarketFX.ShoppingBasket.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace FreemarketFX.ShoppingBasket.API.Tests.Controllers;
public class BasketsControllerTests
{
    private readonly ILogger<BasketsController> _logger;
    private readonly IBasketService _service;
    private readonly Guid _validId = Guid.NewGuid();
    private readonly Guid _invalidId = Guid.NewGuid();
    private readonly Guid _exceptionId = Guid.NewGuid();

    public BasketsControllerTests()
    {
        _logger = Substitute.For<ILogger<BasketsController>>();
        _service = Substitute.For<IBasketService>();

        _service.AddBasketAsync().Returns(
            Task.FromResult<BasketDto>(
            new() { Id = Guid.NewGuid() }));

        _service.GetBasketByIdAsync(_validId).Returns(
            Task.FromResult<BasketDto?>(
            new() { Id = _validId }));

        _service.GetBasketByIdAsync(_invalidId).Returns(
            Task.FromResult<BasketDto?>(
                null));

        _service.GetBasketByIdAsync(_exceptionId).Throws(
            new Exception("A database error has occurred."));
    }

    [Fact]
    public async Task InsertBasket_ShouldReturnInsertedBasket()
    {
        // Arrange
        BasketsController controller = new(_service, _logger);
        ActionResult<BasketDto>? result;

        // Act
        result = await controller.InsertBasket();

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var actualBasket = Assert.IsType<BasketDto>(createdResult.Value);

        Assert.NotNull(actualBasket);
        Assert.NotEqual(Guid.Empty, actualBasket.Id);
        Assert.Empty(actualBasket.Products);
    }

    [Fact]
    public async Task GetBasketById_ShouldReturnBasket_ForValidId()
    {
        // Arrange
        BasketsController controller = new(_service, _logger);
        ActionResult<BasketDto>? result;
        Guid expectedId = _validId;

        // Act
        result = await controller.GetBasketById(expectedId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualBasket = Assert.IsType<BasketDto>(okResult.Value);

        Assert.NotNull(actualBasket);
        Assert.Equal(expectedId, actualBasket.Id);
        Assert.Empty(actualBasket.Products);
    }

    [Fact]
    public async Task GetBasketById_ShouldReturnNull_ForInvalidId()
    {
        // Arrange
        BasketsController controller = new(_service, _logger);
        ActionResult<BasketDto>? result;
        Guid expectedId = _invalidId;

        // Act
        result = await controller.GetBasketById(expectedId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetBasketById_ShouldReturnProblem_WhenErrorOccurs()
    {
        // Arrange
        BasketsController controller = new(_service, _logger);
        ActionResult<BasketDto>? result;
        Guid expectedId = _exceptionId;
        string expectedErrorMessage = "There was an error retrieving the basket.";

        // Act
        result = await controller.GetBasketById(expectedId);

        // Assert
        var problemResult = Assert.IsType<ObjectResult>(result.Result);
        var problemDetails = Assert.IsType<ProblemDetails>(problemResult.Value);

        Assert.Equal(expectedErrorMessage, problemDetails.Detail);
    }
}
