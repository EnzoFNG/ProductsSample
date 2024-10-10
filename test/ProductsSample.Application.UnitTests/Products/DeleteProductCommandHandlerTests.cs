using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ProductsSample.Abstractions.Enums;
using ProductsSample.Application.Products.Delete;
using ProductsSample.Domain.Products;
using ProductsSample.Domain.Products.Errors;
using ProductsSample.Infrastructure.Context;

namespace ProductsSample.Application.UnitTests.Products;

public sealed class DeleteProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnFail_WhenProductDoesNotExist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDeleteProductDb")
            .Options;

        var dbContext = new ProductDbContext(options);
        var handler = new DeleteProductCommandHandler(dbContext);

        // Act
        var command = new DeleteProductCommand { Id = Ulid.NewUlid().ToString() }; // Non-existent ID
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse(); // Should return failure when the product doesn't exist
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenProductNotRemovedDueToSaveChangesFailure()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(databaseName: "TestFailedDeleteProductDb")
            .Options;

        var dbContext = new TestProductDbContext(options); // Uses a custom class that simulates a SaveChanges failure
        SeedDatabase(dbContext); // Populate the in-memory database with a test product

        var handler = new DeleteProductCommandHandler(dbContext);

        var command = new DeleteProductCommand
        {
            Id = dbContext.Products.First().Id.ToString()
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse(); // Should return failure if SaveChanges fails
        result.Response.Should().BeOfType<ProductNotRemovedError>(); // Response should be of type ProductNotRemovedError
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenProductIsDeletedSuccessfully()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDeleteProductDb")
            .Options;

        var dbContext = new ProductDbContext(options);
        SeedDatabase(dbContext); // Seed the database with a product

        var handler = new DeleteProductCommandHandler(dbContext);

        var command = new DeleteProductCommand
        {
            Id = dbContext.Products.First().Id.ToString()
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue(); // Should return success if the product is deleted successfully
        result.Response.Should().Be("The product has been deleted successfully!");

        // Verify the product has been deactivated
        var deletedProduct = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == Ulid.Parse(command.Id));
        deletedProduct.Should().NotBeNull();
        deletedProduct!.Status.Should().Be(EntityStatus.Inactive); // Check if the product status is set to Inactive
    }

    private static void SeedDatabase(ProductDbContext dbContext)
    {
        if (dbContext.Products.Any())
            return;

        // Create a product and add it to the database
        var product = Product.Create("Test Product", 100m, 10).Response as Product;
        dbContext.Products.Add(product!);
        dbContext.SaveChanges();
    }

    private class TestProductDbContext(DbContextOptions<ProductDbContext> options) 
        : ProductDbContext(options)
    {
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var random = new Random().Next(0, 1);
            if (random == 1)
                return Task.FromResult(0);

            throw new DbUpdateException();
        }
    }
}
