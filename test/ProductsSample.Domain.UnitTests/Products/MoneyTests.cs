using ProductsSample.Domain.Products;
using FluentAssertions;

namespace ProductsSample.Domain.UnitTests.Products;

public sealed class MoneyTests
{
    [Fact]
    public void Money_Zero_ShouldReturnMoneyWithZeroValue()
    {
        // Arrange
        var expectedValue = decimal.Zero;

        // Act
        var zeroMoney = Money.Zero;

        // Assert
        zeroMoney.Value.Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(100)]
    [InlineData(500)]
    [InlineData(0.01)]
    public void Money_AssignValue_ShouldCreateMoneyWithGivenValue(decimal value)
    {
        // Act
        var money = Money.AssignValue(value);

        // Assert
        money.Value.Should().Be(value);
    }

    [Fact]
    public void ImplicitConversion_ToDecimal_ShouldReturnDecimalValue()
    {
        // Arrange
        var money = Money.AssignValue(150);

        // Act
        decimal decimalValue = money;

        // Assert
        decimalValue.Should().Be(150);
    }

    [Fact]
    public void ImplicitConversion_FromDecimal_ShouldReturnMoneyObject()
    {
        // Arrange
        decimal decimalValue = 200;

        // Act
        Money money = decimalValue;

        // Assert
        money.Value.Should().Be(decimalValue);
    }

    [Fact]
    public void Money_ShouldBeInvalid_WhenValueIsNegative()
    {
        // Act
        Money negativeMoney = -10;

        // Assert
        negativeMoney.IsValid.Should().Be(false);
    }

    [Fact]
    public void Money_Validate_ShouldThrowError_WhenValueIsNegative()
    {
        // Act
        var negativeMoney = Money.AssignValue(-5);

        // Assert
        negativeMoney.Notifications.Should().ContainSingle(n => n.Message == "The money value cannot be lower than 0.");
    }
}
