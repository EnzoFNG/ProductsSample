using FluentAssertions;
using ProductsSample.Abstractions.Enums;
using ProductsSample.Domain.Products;

namespace ProductsSample.Domain.UnitTests.Products;

public class ProductSpecificationsTests
{
    private readonly List<Product> _products;

    public ProductSpecificationsTests()
    {
        _products =
        [
            Product.Create("Product A", 50m, 100).Response as Product,
            Product.Create("Product B", 30m, 50).Response as Product,
            Product.Create("Product C", 100m, 200).Response as Product,
            Product.Create("Product D", 10m, 10).Response as Product
        ];

        _products.FirstOrDefault(x => x.Name == "Product B")!.Deactivate();
    }

    [Fact]
    public void ById_ShouldReturnProduct_WhenProductIdMatches()
    {
        // Arrange
        var product = _products.First(p => p.Status == EntityStatus.Active);
        var spec = ProductSpecifications.ById(product.Id);

        // Act
        var result = _products.AsQueryable().Where(spec).ToList();

        // Assert
        result.Should().ContainSingle().Which.Should().Be(product);
    }

    [Fact]
    public void ById_ShouldNotReturnProduct_WhenProductIsInactive()
    {
        // Arrange
        var inactiveProduct = _products.First(p => p.Status == EntityStatus.Inactive);
        var spec = ProductSpecifications.ById(inactiveProduct.Id);

        // Act
        var result = _products.AsQueryable().Where(spec).ToList();

        // Assert
        result.Should().BeEmpty();
    }


    [Fact]
    public void ByExactName_ShouldReturnProduct_WhenNameMatches()
    {
        // Arrange
        string name = "Product A";
        var spec = ProductSpecifications.ByExactName(name);

        // Act
        var result = _products.AsQueryable().Where(spec).ToList();

        // Assert
        result.Should().ContainSingle().Which.Name.Should().Be(name);
    }

    [Fact]
    public void ByExactName_ShouldReturnNull_WhenNamePartiallyMatches()
    {
        // Arrange
        string name = "Product";
        var spec = ProductSpecifications.ByExactName(name);

        // Act
        var result = _products.AsQueryable().Where(spec).ToList();

        // Assert
        result.Should().HaveCount(0);
    }

    [Fact]
    public void ByName_ShouldReturnProduct_WhenNameMatches()
    {
        // Arrange
        string name = "Product A";
        var spec = ProductSpecifications.ByName(name);

        // Act
        var result = _products.AsQueryable().Where(spec).ToList();

        // Assert
        result.Should().ContainSingle().Which.Name.Should().Be(name);
    }

    [Fact]
    public void ByName_ShouldReturnProducts_WhenNamePartiallyMatches()
    {
        // Arrange
        string name = "Product";
        var spec = ProductSpecifications.ByName(name);

        // Act
        var result = _products.AsQueryable().Where(spec).ToList();

        // Assert
        result.Should().HaveCount(3);
    }

    [Fact]
    public void ByMinimumPrice_ShouldReturnProductsAboveMinimumPrice()
    {
        // Arrange
        decimal minimumPrice = 30m;
        var spec = ProductSpecifications.ByMinimumPrice(minimumPrice);

        // Act
        var result = _products.AsQueryable().Where(spec).ToList();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public void ByMaximumPrice_ShouldReturnProductsBelowMaximumPrice()
    {
        // Arrange
        decimal maximumPrice = 50m;
        var spec = ProductSpecifications.ByMaximumPrice(maximumPrice);

        // Act
        var result = _products.AsQueryable().Where(spec).ToList();

        // Assert
        result.Should().HaveCount(2); 
    }

    [Fact]
    public void ByMinimumStock_ShouldReturnProductsAboveMinimumStock()
    {
        // Arrange
        int minimumStock = 50;
        var spec = ProductSpecifications.ByMinimumStock(minimumStock);

        // Act
        var result = _products.AsQueryable().Where(spec).ToList();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public void ByMaximumStock_ShouldReturnProductsBelowMaximumStock()
    {
        // Arrange
        int maximumStock = 50;
        var spec = ProductSpecifications.ByMaximumStock(maximumStock);

        // Act
        var result = _products.AsQueryable().Where(spec).ToList();

        // Assert
        result.Should().HaveCount(1);
    }
}
