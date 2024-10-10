using FluentAssertions;
using ProductsSample.Domain.Products;

namespace ProductsSample.Domain.UnitTests.Products;

public sealed class StockTests
{
    [Fact]
    public void Stock_Zero_ShouldReturnStockWithZeroValue()
    {
        // Arrange
        var expectedValue = 0u;

        // Act
        var zeroStock = Stock.Zero;

        // Assert
        zeroStock.Value.Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    public void Stock_Create_ShouldCreateStockWithGivenValue(uint value)
    {
        // Act
        var stock = Stock.Create(value);

        // Assert
        stock.IsValid.Should().Be(true);
        stock.Value.Should().Be(value);
    }

    [Fact]
    public void ImplicitConversion_ToUInt_ShouldReturnUIntValue()
    {
        // Arrange
        var stock = Stock.Create(150);

        // Act
        uint uintValue = stock;

        // Assert
        uintValue.Should().Be(150);
    }

    [Fact]
    public void ImplicitConversion_ToInt_ShouldReturnIntValue()
    {
        // Arrange
        var stock = Stock.Create(120);

        // Act
        int intValue = stock;

        // Assert
        intValue.Should().Be(120);
    }

    [Fact]
    public void ImplicitConversion_FromUInt_ShouldReturnStockObject()
    {
        // Arrange
        uint uintValue = 200;

        // Act
        Stock stock = uintValue;

        // Assert
        stock.Value.Should().Be(uintValue);
    }

    [Fact]
    public void ImplicitConversion_FromInt_ShouldReturnStockObject()
    {
        // Arrange
        int intValue = 300;

        // Act
        Stock stock = intValue;

        // Assert
        stock.Value.Should().Be((uint)intValue);
    }

    [Fact]
    public void Stock_ShouldReturnZero_WhenValueIsZeroOrLess()
    {
        // Act
        Stock negativeStock = -5;
        Stock zeroStock = 0;

        // Assert
        negativeStock.Value.Should().Be(0);
        zeroStock.Value.Should().Be(0);
    }
}