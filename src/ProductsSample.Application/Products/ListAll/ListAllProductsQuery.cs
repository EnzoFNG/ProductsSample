using ProductsSample.Abstractions.Primitives;
using ProductsSample.Abstractions.Queries;

namespace ProductsSample.Application.Products.ListAll;

public sealed class ListAllProductsQuery : PaginatedQuery<Result>
{
    public string? Name { get; set; }
    public decimal? MinimumPrice { get; set; }
    public decimal? MaximumPrice { get; set; }
    public int? MinimumStock { get; set; }
    public int? MaximumStock { get; set; }

    public override void Validate()
    {
        ValidatePaginate();

        if (Name is not null and "")
        {
            AddNotification(Key, "Product name cannot be empty.");
        }

        if (MinimumPrice is not null and < 0)
        {
            AddNotification(Key, "Minimum price cannot be lower than 0.");
        }

        if (MaximumPrice is not null and < 0 || MinimumPrice < MinimumPrice)
        {
            AddNotification(Key, "Maximum price cannot be lower than 0 or minimum price.");
        }

        if (MinimumStock is not null and < 0)
        {
            AddNotification(Key, "Product name cannot be empty.");
        }

        if (MaximumStock is not null and < 0 || MaximumStock < MinimumStock)
        {
            AddNotification(Key, "Product name cannot be empty.");
        }
    }
}