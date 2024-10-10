namespace ProductsSample.Application.Products.ListAll;

public sealed record ListAllProductsQueryResponseItem
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; } 
    public int Stock { get; set; }
}