using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ProductsSample.Application.Products.ListAll;
using ProductsSample.Domain.Products;
using ProductsSample.Infrastructure.Context;

namespace ProductsSample.Application.UnitTests.Products;

public sealed class ListAllProductsQueryHandlerTests
{
    private readonly ProductDbContext _dbContext;
    private readonly ListAllProductsQueryHandler _handler;

    public ListAllProductsQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(databaseName: "AnotherTestProductDb")
            .Options;

        _dbContext = new ProductDbContext(options);

        SeedDatabase(_dbContext);

        _handler = new ListAllProductsQueryHandler(_dbContext);
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
            (Product.Create("Product D", 70.0m, 200).Response as Product)!
        };

        context.Products.AddRange(products);
        context.SaveChanges();
    }

    [Fact]
    public async Task Handle_ShouldReturnAllProducts_WhenNoFiltersApplied()
    {
        // Arrange
        var query = new ListAllProductsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var response = result.Response as ListAllProductsQueryResponse;
        response!.Products.Should().HaveCount(4);
        response.TotalItems.Should().Be(4);
    }

    [Fact]
    public async Task Handle_ShouldReturnFilteredProducts_ByName()
    {
        // Arrange
        var query = new ListAllProductsQuery { Name = "Product A" };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var response = result.Response as ListAllProductsQueryResponse;
        response!.Products.Should().HaveCount(1);
        response.Products.First().Name.Should().Be("Product A");
    }

    [Fact]
    public async Task Handle_ShouldReturnFilteredProducts_ByMinimumPrice()
    {
        // Arrange
        var query = new ListAllProductsQuery { MinimumPrice = 50.0m };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var response = result.Response as ListAllProductsQueryResponse;
        response!.Products.Should().HaveCount(2);  // Products C and D have the same price >= 50
        response.Products.Should().Contain(p => p.Name == "Product C");
        response.Products.Should().Contain(p => p.Name == "Product D");
    }

    [Fact]
    public async Task Handle_ShouldReturnFilteredProducts_ByMaximumPrice()
    {
        // Arrange
        var query = new ListAllProductsQuery { MaximumPrice = 25.0m };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var response = result.Response as ListAllProductsQueryResponse;
        response!.Products.Should().HaveCount(2);  // Products A and B have the same price <= 25
        response.Products.Should().Contain(p => p.Name == "Product A");
        response.Products.Should().Contain(p => p.Name == "Product B");
    }

    [Fact]
    public async Task Handle_ShouldReturnFilteredProducts_ByMinimumStock()
    {
        // Arrange
        var query = new ListAllProductsQuery { MinimumStock = 100 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var response = result.Response as ListAllProductsQueryResponse;
        response!.Products.Should().HaveCount(2);  // Products C and D have the stock value >= 100
        response.Products.Should().Contain(p => p.Name == "Product C");
        response.Products.Should().Contain(p => p.Name == "Product D");
    }

    [Fact]
    public async Task Handle_ShouldReturnPaginatedProducts()
    {
        // Arrange
        var query = new ListAllProductsQuery { Page = 1, Size = 2 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var response = result.Response as ListAllProductsQueryResponse;
        response!.Products.Should().HaveCount(2);  // First page of 2 products
        response.TotalItems.Should().Be(4);  // Total count is still 4
    }

    [Fact]
    public async Task Handle_ShouldReturnCorrectTotalItems_WithFilters()
    {
        // Arrange
        var query = new ListAllProductsQuery { MinimumPrice = 50.0m, MaximumStock = 100 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var response = result.Response as ListAllProductsQueryResponse;
        response!.Products.Should().HaveCount(1);  // Just the "Product C" meets these criteria
        response.TotalItems.Should().Be(4);  // Total count is still 4
    }
}
