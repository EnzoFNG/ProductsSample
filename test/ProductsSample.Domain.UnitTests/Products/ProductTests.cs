using FluentAssertions;
using Flunt.Notifications;
using ProductsSample.Abstractions.Primitives;
using ProductsSample.Domain.Products;

namespace ProductsSample.Domain.UnitTests.Products;

public sealed class ProductTests
{
    [Fact]
    public void Product_Create_ShouldReturnSuccessResult_WhenValidData()
    {
        // Arrange
        string name = "Product A";
        decimal price = 50m;
        uint stock = 100;

        // Act
        var result = Product.Create(name, price, stock);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var product = result.Response as Product;

        product.Should().NotBeNull();
        product!.Name.Should().Be(name);
        product.Price.Value.Should().Be(price);
        product.Stock.Value.Should().Be(stock);
    }

    [Fact]
    public void Product_Create_ShouldReturnFailResult_WhenNameIsEmpty()
    {
        // Arrange
        string name = string.Empty;
        decimal price = 50m;
        uint stock = 100;

        // Act
        var result = Product.Create(name, price, stock);

        // Assert
        var notifications = (result.Response as List<Notification>)!;

        result.IsSuccess.Should().BeFalse();
        notifications
            .Should()
            .ContainSingle(e => e.Message == "The product name cannot be empty.");
    }

    [Fact]
    public void Product_Create_ShouldUseZeroStock_WhenStockIsNotProvided()
    {
        // Arrange
        string name = "Product B";
        decimal price = 75m;

        // Act
        var result = Product.Create(name, price);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var product = result.Response as Product;

        product!.Stock.Should().Be(Stock.Zero);
    }

    [Fact]
    public void Product_Edit_ShouldUpdateProductDetails_WhenValidData()
    {
        // Arrange
        var product = Product.Create("Product A", 50m, 100).Response as Product;
        string newName = "Product A Updated";
        decimal newPrice = 100m;
        uint newStock = 200;

        // Act
        var result = product!.Edit(newName, newPrice, newStock);

        // Assert
        result.IsSuccess.Should().BeTrue();
        product.Name.Should().Be(newName);
        product.Price.Value.Should().Be(newPrice);
        product.Stock.Value.Should().Be(newStock);
    }

    [Fact]
    public void Product_Edit_ShouldReturnFailResult_WhenNameIsEmpty()
    {
        // Arrange
        var product = Product.Create("Product A", 50m, 100).Response as Product;
        string newName = string.Empty;
        decimal newPrice = 100m;
        uint newStock = 200;

        // Act
        var result = product!.Edit(newName, newPrice, newStock);

        // Assert
        result.IsSuccess.Should().BeFalse();
        (result.Response as List<Notification>).Should().ContainSingle(e => e.Message == "The product name cannot be empty.");
    }

    [Fact]
    public void Product_ShouldFail_WhenPriceIsInvalid()
    {
        // Arrange
        string name = "Invalid Product";
        decimal invalidPrice = -50m;

        // Act
        var result = Product.Create(name, invalidPrice);

        // Assert
        var notifications = (result.Response as List<Notification>)!;

        result.IsSuccess.Should().BeFalse();
        notifications.Should().Contain(e => e.Message == "The money value cannot be lower than 0.");
    }

    [Fact]
    public void Product_ImplicitConversion_ShouldReturnSuccessResult_WhenProductIsValid()
    {
        // Arrange
        var product = Product.Create("Valid Product", 50m, 100).Response as Product;

        // Act
        Result result = product!;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Response.Should().Be(product);
    }

    [Fact]
    public void Product_ImplicitConversion_ShouldReturnFailResult_WhenProductIsInvalid()
    {
        // Arrange
        var result = Product.Create(string.Empty, 50m, 100);

        // Assert
        result.IsSuccess.Should().BeFalse();
        (result.Response as List<Notification>).Should().ContainSingle(e => e.Message == "The product name cannot be empty.");
    }
}