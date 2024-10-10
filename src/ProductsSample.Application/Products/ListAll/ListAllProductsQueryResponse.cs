namespace ProductsSample.Application.Products.ListAll;

public sealed record ListAllProductsQueryResponse
{
    public IEnumerable<ListAllProductsQueryResponseItem> Products { get; set; } = [];
    public long TotalItems { get; set; }
}