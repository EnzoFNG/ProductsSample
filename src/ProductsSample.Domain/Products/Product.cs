using ProductsSample.Abstractions.Extensions;
using ProductsSample.Abstractions.Primitives;

namespace ProductsSample.Domain.Products;

public sealed class Product : BaseEntity<Product>
{
    private Product()
    { }

    private Product(string name, Money price, Stock stock)
   {
        Name = name;
        Price = price;
        Stock = stock;
        Validate();
    }

    public string Name { get; private set; } = string.Empty;
    public Money Price { get; private set; } = Money.Zero;
    public Stock Stock { get; private set; } = uint.MinValue;

    public static Result Create(string name, decimal price, uint? stock = null)
    {
        var product = new Product(name, price, stock ?? Stock.Zero);

        return product;
    }

    public Result Edit(string name, decimal price, uint stock)
    {
        Name = name;
        Price = price;
        Stock = stock;
        Validate();

        return this;
    }

    public static implicit operator Result(Product product)
    {
        if (!product.IsValid)
            return Result.Fail(product.Notifications);

        return Result.Success(product);
    }

    protected override void Validate()
    {
        AddNotifications(Price, Stock);

        AddNotifications(ContractRequires.IsFalse(Name.IsEmpty(), Key, "The product name cannot be empty."));
    }
}