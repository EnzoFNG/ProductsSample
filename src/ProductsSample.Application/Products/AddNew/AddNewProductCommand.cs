using ProductsSample.Abstractions.Commands;
using ProductsSample.Abstractions.Primitives;

namespace ProductsSample.Application.Products.AddNew;

public sealed class AddNewProductCommand : Command<Result>
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public uint Stock { get; set; }

    protected override void Validate()
    {
        return;
    }
}