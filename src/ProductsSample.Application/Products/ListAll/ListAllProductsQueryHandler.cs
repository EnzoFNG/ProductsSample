using Microsoft.EntityFrameworkCore;
using ProductsSample.Abstractions.Extensions;
using ProductsSample.Abstractions.Handlers;
using ProductsSample.Abstractions.Primitives;
using ProductsSample.Domain.Products;
using ProductsSample.Infrastructure.Context;

namespace ProductsSample.Application.Products.ListAll;

public sealed class ListAllProductsQueryHandler(ProductDbContext dbContext) : IQueryHandler<ListAllProductsQuery, Result>
{
    public async Task<Result> Handle(ListAllProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .When(query.Name is not null, ProductSpecifications.ByName(query.Name ?? string.Empty))
            .When(query.MinimumPrice is not null, ProductSpecifications.ByMinimumPrice(query.MinimumPrice ?? 0))
            .When(query.MaximumPrice is not null, ProductSpecifications.ByMaximumPrice(query.MaximumPrice ?? 0))
            .When(query.MinimumStock is not null, ProductSpecifications.ByMinimumStock(query.MinimumStock ?? 0))
            .When(query.MaximumStock is not null, ProductSpecifications.ByMaximumStock(query.MaximumStock ?? 0))
            .Paginate(query.Size, query.Skip)
            .Select(x => new ListAllProductsQueryResponseItem
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Price = x.Price,
                Stock = x.Stock,
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var count = await dbContext.Products.LongCountAsync(ProductSpecifications.ByActive(), cancellationToken);

        var response = new ListAllProductsQueryResponse()
        {
            Products = products,
            TotalItems = count
        };

        return Result.Success(response);
    }
}