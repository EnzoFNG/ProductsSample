using Microsoft.EntityFrameworkCore;
using ProductsSample.Abstractions.Handlers;
using ProductsSample.Abstractions.Primitives;
using ProductsSample.Domain.Products;
using ProductsSample.Domain.Products.Errors;
using ProductsSample.Infrastructure.Context;

namespace ProductsSample.Application.Products.Delete;

public sealed class DeleteProductCommandHandler(ProductDbContext dbContext) : ICommandHandler<DeleteProductCommand, Result>
{
    public async Task<Result> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var productId = Ulid.Parse(command.Id);
        var existentProduct = await dbContext.Products.FirstOrDefaultAsync(ProductSpecifications.ById(productId), cancellationToken);

        if (existentProduct is null)
            return Result.Fail();

        existentProduct.Deactivate();

        try
        {
            dbContext.Products.Update(existentProduct);

            var success = await dbContext.SaveChangesAsync(cancellationToken) > 0;

            if (!success)
                return Result.Fail(new ProductNotRemovedError());
        }
        catch (Exception)
        {
            return Result.Fail(new ProductNotRemovedError());
        }

        return Result.Success("The product has been deleted successfully!");
    }
}