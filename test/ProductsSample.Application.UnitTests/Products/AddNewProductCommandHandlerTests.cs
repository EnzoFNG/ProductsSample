using FluentAssertions;
using Flunt.Notifications;
using Microsoft.EntityFrameworkCore;
using ProductsSample.Application.Products.AddNew;
using ProductsSample.Domain.Products;
using ProductsSample.Domain.Products.Errors;
using ProductsSample.Infrastructure.Context;

namespace ProductsSample.Application.UnitTests.Products;

public sealed class AddNewProductCommandHandlerTests
{
    private readonly ProductDbContext _dbContext;
    private readonly AddNewProductCommandHandler _handler;

    public AddNewProductCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new ProductDbContext(options);
        _handler = new AddNewProductCommandHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenProductAlreadyExists()
    {
        // Arrange
        var command = new AddNewProductCommand { Name = "Existing Product", Price = 100m, Stock = 10 };

        var existingProduct = Product.Create(command.Name, command.Price, command.Stock).Response as Product;
        await _dbContext.Products.AddAsync(existingProduct!, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Response.Should().BeOfType<ProductAlreadyExistsError>();
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenProductCreationFails()
    {
        // Arrange
        var command = new AddNewProductCommand { Name = string.Empty, Price = -100m, Stock = 0 };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Response.Should().BeAssignableTo<List<Notification>>();
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenProductIsAddedSuccessfully()
    {
        // Arrange
        var command = new AddNewProductCommand { Name = "New Product", Price = 100m, Stock = 10 };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Response.Should().Be("The product has been added successfully!");

        var addedProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Name == command.Name);
        addedProduct.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenProductNotAddedToDatabase()
    {
        // Arrange
        var command = new AddNewProductCommand { Name = "New Product", Price = 100m, Stock = 10 };

        var existingProduct = Product.Create(command.Name, command.Price, command.Stock).Response as Product;
        await _dbContext.Products.AddAsync(existingProduct!, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Response.Should().BeOfType<ProductAlreadyExistsError>();
    }
}