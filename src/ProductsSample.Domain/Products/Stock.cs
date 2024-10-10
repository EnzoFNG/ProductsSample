using ProductsSample.Abstractions.Primitives;
using System.Diagnostics.CodeAnalysis;

namespace ProductsSample.Domain.Products;

public sealed class Stock : ValueObject<Stock>, IEqualityComparer<Stock>
{
    private Stock()
    { }

    private Stock(uint value)
    {
        Value = value;
        Validate();
    }

    public uint Value { get; private set; } = uint.MinValue;

    public static Stock Zero => new(0);

    public static Stock Create(uint value)
    {
        var stock = new Stock(value);

        return stock;
    }

    public static implicit operator uint(Stock value) => value.Value;

    public static implicit operator int(Stock value) => (int)value.Value;

    public static implicit operator Stock(uint value)
    {
        if (value <= 0)
            return Zero;

        return Create(value);
    }
    public static implicit operator Stock(int value)
    {
        if (value <= 0)
            return Zero;

        return Create((uint)value);
    }

    protected override void Validate()
    {
        return;
    }

    public bool Equals(Stock? x, Stock? y)
    {
        throw new NotImplementedException();
    }

    public int GetHashCode([DisallowNull] Stock obj)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object? obj)
        => obj is Stock stock && Value.Equals(stock.Value);

    public override int GetHashCode()
        => HashCode.Combine(Value);
}