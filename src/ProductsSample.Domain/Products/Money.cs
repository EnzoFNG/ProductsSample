using ProductsSample.Abstractions.Primitives;

namespace ProductsSample.Domain.Products;

public sealed class Money : ValueObject<Money>
{
    private Money()
    { }

    private Money(decimal value)
    {
        Value = value;
        Validate();
    }

    public decimal Value { get; private set; } = decimal.Zero;

    public static Money Zero => new(0);

    public static Money AssignValue(decimal value)
    {
        var money = new Money(value);

        return money;
    }

    public static implicit operator decimal(Money value) => value.Value;

    public static implicit operator Money(decimal value)
    {
        return AssignValue(value);
    }

    protected override void Validate()
    {
        AddNotifications(ContractRequires.IsTrue(Value >= 0, Key, "The money value cannot be lower than 0."));
    }
}