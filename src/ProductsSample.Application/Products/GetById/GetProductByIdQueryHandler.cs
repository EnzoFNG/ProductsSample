using Microsoft.EntityFrameworkCore;
using ProductsSample.Abstractions.Handlers;
using ProductsSample.Abstractions.Primitives;
using ProductsSample.Domain.Products;
using ProductsSample.Infrastructure.Context;

namespace ProductsSample.Application.Products.GetById;

public sealed class GetProductByIdQueryHandler(ProductDbContext dbContext) : IQueryHandler<GetProductByIdQuery, Result>
{
    public async Task<Result> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var productId = Ulid.Parse(query.Id);
        var existentProduct = await dbContext.Products.FirstOrDefaultAsync(ProductSpecifications.ById(productId), cancellationToken);

        if (existentProduct is null)
            return Result.Fail();

        var response = new GetProductByIdQueryResponse
        {
            Id = existentProduct.Id.ToString(),
            Name = existentProduct.Name,
            Price = existentProduct.Price,
            Stock = existentProduct.Stock,
        };

        return Result.Success(response);
    }
}