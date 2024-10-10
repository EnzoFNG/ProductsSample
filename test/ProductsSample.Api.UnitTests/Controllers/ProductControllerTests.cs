using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductsSample.Abstractions.Mediator;
using ProductsSample.Abstractions.Primitives;
using ProductsSample.Api.Controllers;
using ProductsSample.Application.Products.AddNew;
using ProductsSample.Application.Products.Delete;
using ProductsSample.Application.Products.EditDetails;
using ProductsSample.Application.Products.GetById;
using ProductsSample.Application.Products.ListAll;
using static ProductsSample.Abstractions.Controllers.CustomController;

namespace ProductsSample.Api.UnitTests.Controllers;

public class ProductControllerTests
{
    private readonly Mock<IMediatorHandler> _mediatorMock;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _mediatorMock = new Mock<IMediatorHandler>();
        _controller = new ProductController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetProductByIdAsync_ShouldReturnOk_WhenProductExists()
    {
        // Arrange: Mock a successful result with a product object
        var response = new GetProductByIdQueryResponse
        {
            Id = Ulid.NewUlid().ToString(),
            Name = "Test Product",
            Price = 100m,
            Stock = 50,
        };

        var expectedResponse = Result.Success(response);

        _mediatorMock
            .Setup(m => m.SendQueryAsync(It.IsAny<GetProductByIdQuery>()))
            .ReturnsAsync(expectedResponse);

        // Act: Call the controller's GetProductByIdAsync method
        var result = await _controller.GetProductByIdAsync(response.Id);

        // Assert: Verify that the response is OK and contains the expected product
        var okResult = result as OkObjectResult;
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(expectedResponse.Response);
    }

    [Fact]
    public async Task GetProductByIdAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange: Mock a result where the product is not found
        var expectedResponse = Result.Fail();

        _mediatorMock
            .Setup(m => m.SendQueryAsync(It.IsAny<GetProductByIdQuery>()))
            .ReturnsAsync(expectedResponse);

        // Act: Call the controller's GetProductByIdAsync method
        var result = await _controller.GetProductByIdAsync("123");

        // Assert: Verify that the response is NotFound
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task ListAllProductsAsync_ShouldReturnOk_WithListOfProducts()
    {
        // Arrange: Mock a successful result with a list of products
        var response = new List<ListAllProductsQueryResponseItem>
        {
            new() 
            {
                Id = Ulid.NewUlid().ToString(),
                Name = "Product A",
                Price = 100m,
                Stock = 50,
            },
            new()
            {
                Id = Ulid.NewUlid().ToString(),
                Name = "Product B",
                Price = 70m,
                Stock = 30,
            }, 
            new()
            {
                Id = Ulid.NewUlid().ToString(),
                Name = "Product C",
                Price = 200m,
                Stock = 90,
            }
        };

        var expectedResponse = Result.Success(response);

        _mediatorMock
            .Setup(m => m.SendQueryAsync(It.IsAny<ListAllProductsQuery>()))
            .ReturnsAsync(expectedResponse);

        // Act: Call the controller's ListAllProductsAsync method
        var result = await _controller.ListAllProductsAsync(new ListAllProductsQuery());

        // Assert: Verify that the response is OK and contains the expected list of products
        var okResult = result as OkObjectResult;
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(expectedResponse.Response);
    }

    [Fact]
    public async Task AddNewProductAsync_ShouldReturnOk_WhenProductIsAddedSuccessfully()
    {
        // Arrange: Mock a successful result when adding a new product
        var expectedResponse = Result.Success("Product created successfully");

        _mediatorMock
            .Setup(m => m.SendCommandAsync(It.IsAny<AddNewProductCommand>()))
            .ReturnsAsync(expectedResponse);

        // Act: Call the controller's AddNewProductAsync method
        var result = await _controller.AddNewProductAsync(new AddNewProductCommand());

        // Assert: Verify that the response is OK and contains the success message
        var okResult = result as OkObjectResult;
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().Be(new SuccessResponse(expectedResponse.Response!.ToString()!));
    }

    [Fact]
    public async Task EditProductDetailsAsync_ShouldReturnOk_WhenProductIsEditedSuccessfully()
    {
        // Arrange: Mock a successful result when editing a product
        var expectedResponse = Result.Success("Product updated successfully");

        _mediatorMock
            .Setup(m => m.SendCommandAsync(It.IsAny<EditProductDetailsCommand>()))
            .ReturnsAsync(expectedResponse);

        // Act: Call the controller's EditProductDetailsAsync method
        var result = await _controller.EditProductDetailsAsync("123", new EditProductDetailsCommand());

        // Assert: Verify that the response is OK and contains the success message
        var okResult = result as OkObjectResult;
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().Be(new SuccessResponse(expectedResponse.Response!.ToString()!));
    }

    [Fact]
    public async Task DeleteProductAsync_ShouldReturnOk_WhenProductIsDeletedSuccessfully()
    {
        // Arrange: Mock a successful result when deleting a product
        var expectedResponse = Result.Success("Product deleted successfully");

        _mediatorMock
            .Setup(m => m.SendCommandAsync(It.IsAny<DeleteProductCommand>()))
            .ReturnsAsync(expectedResponse);

        // Act: Call the controller's DeleteProductAsync method
        var result = await _controller.DeleteProductAsync("123");

        // Assert: Verify that the response is OK and contains the success message
        var okResult = result as OkObjectResult;
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().Be(new SuccessResponse(expectedResponse.Response!.ToString()!));
    }
}
