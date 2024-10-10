using ProductsSample.Abstractions.Commands;
using ProductsSample.Abstractions.Primitives;
using System.Text.Json.Serialization;

namespace ProductsSample.Application.Products.EditDetails;

public sealed class EditProductDetailsCommand : Command<Result>
{
    [JsonIgnore]
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public uint Stock { get; set; }

    protected override void Validate()
    {
        return;
    }
}