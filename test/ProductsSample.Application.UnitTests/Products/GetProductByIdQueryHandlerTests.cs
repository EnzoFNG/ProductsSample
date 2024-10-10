using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ProductsSample.Application.Products.GetById;
using ProductsSample.Domain.Products;
using ProductsSample.Infrastructure.Context;

namespace ProductsSample.Application.UnitTests.Products;

public sealed class GetProductByIdQueryHandlerTests
{
    private readonly ProductDbContext _dbContext;
    private readonly GetProductByIdQueryHandler _handler;

    public GetProductByIdQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(databaseName: "TestProductDb")
            .Options;

        _dbContext = new ProductDbContext(options);

        SeedDatabase(_dbContext);

        _handler = new GetProductByIdQueryHandler(_dbContext);
    }

    private static void SeedDatabase(ProductDbContext context)
    {
        if (context.Products.Any())
            return;

        var products = new List<Product>
            {
                (Product.Create("Product A", 10.0m, 50).Response as Product)!,
                (Product.Create("Product B", 25.0m, 20).Response as Product)!,
                (Product.Create("Product C", 50.0m, 100).Response as Product)!,
            };

        context.Products.AddRange(products);
        context.SaveChanges();
    }

    [Fact]
    public async Task Handle_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var existentProduct = _dbContext.Products.First();
        var query = new GetProductByIdQuery
        {
            Id = existentProduct.Id.ToString()
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var response = result.Response as GetProductByIdQueryResponse;
        response.Should().NotBeNull();
        response!.Id.Should().Be(query.Id);
        response.Name.Should().Be(existentProduct!.Name);
        response.Price.Should().Be(existentProduct.Price);
        response.Stock.Should().Be(existentProduct.Stock);
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenProductDoesNotExist()
    {
        // Arrange
        var query = new GetProductByIdQuery
        {
            Id = "01J9S0PRE9S11HQF4ER1TMXXZZ" // A random ID that doesn't exist
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Response?.Should().BeNull();
    }
}