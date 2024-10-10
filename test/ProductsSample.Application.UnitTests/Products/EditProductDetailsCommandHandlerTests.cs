using FluentAssertions;
using Flunt.Notifications;
using Microsoft.EntityFrameworkCore;
using ProductsSample.Application.Products.EditDetails;
using ProductsSample.Domain.Products;
using ProductsSample.Domain.Products.Errors;
using ProductsSample.Infrastructure.Context;

namespace ProductsSample.Application.UnitTests.Products;

public sealed class EditProductDetailsCommandHandlerTests
{
    private readonly ProductDbContext _dbContext;
    private readonly EditProductDetailsCommandHandler _handler;

    public EditProductDetailsCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(databaseName: "TestEditProductDb")
            .Options;

        _dbContext = new ProductDbContext(options);
        _handler = new EditProductDetailsCommandHandler(_dbContext);

        SeedDatabase(_dbContext);
    }

    private static void SeedDatabase(ProductDbContext context)
    {
        if (!context.Products.Any())
        {
            var product = Product.Create("Existing Product", 100m, 10).Response as Product;
            context.Products.Add(product!);
            context.SaveChanges();
        }
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenProductDoesNotExist()
    {
        // Arrange
        var command = new EditProductDetailsCommand { Id = Ulid.NewUlid().ToString(), Name = "Nonexistent Product", Price = 200m, Stock = 20 };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenUpdateFails()
    {
        // Arrange
        var command = new EditProductDetailsCommand
        {
            Id = _dbContext.Products.First().Id.ToString(),
            Name = string.Empty,
            Price = -100m,
            Stock = 0
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Response.Should().BeOfType<List<Notification>>();
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenProductIsUpdatedSuccessfully()
    {
        // Arrange
        var command = new EditProductDetailsCommand
        {
            Id = _dbContext.Products.First().Id.ToString(),
            Name = "Updated Product",
            Price = 150m,
            Stock = 50
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Response.Should().Be("The product has been updated successfully!");

        var updatedProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Name == command.Name);
        updatedProduct.Should().NotBeNull();
        updatedProduct!.Price.Value.Should().Be(command.Price);
        updatedProduct.Stock.Value.Should().Be(command.Stock);
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenProductNotUpdated()
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(databaseName: "TestEditProductDb")
            .Options;

        var dbContext = new TestProductDbContext(options);
        var handler = new EditProductDetailsCommandHandler(dbContext);

        SeedDatabase(dbContext); // Assume this method adds an existing product

        // Arrange
        var command = new EditProductDetailsCommand
        {
            Id = dbContext.Products.First().Id.ToString(),
            Name = "New Product",
            Price = 100m,
            Stock = 10
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Response.Should().BeOfType<ProductNotUpdatedError>();
    }

    private class TestProductDbContext(DbContextOptions<ProductDbContext> options)
        : ProductDbContext(options)
    {
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var random = new Random().NextInt64(0, 1);

            // Simulate a failure
            if (random == 1)
                return Task.FromResult(0);

            throw new DbUpdateException();
        }
    }
}