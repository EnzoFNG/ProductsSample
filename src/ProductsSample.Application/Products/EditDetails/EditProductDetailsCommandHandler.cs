using Microsoft.EntityFrameworkCore;
using ProductsSample.Abstractions.Handlers;
using ProductsSample.Abstractions.Primitives;
using ProductsSample.Domain.Products;
using ProductsSample.Domain.Products.Errors;
using ProductsSample.Infrastructure.Context;

namespace ProductsSample.Application.Products.EditDetails;

public sealed class EditProductDetailsCommandHandler(ProductDbContext dbContext) : ICommandHandler<EditProductDetailsCommand, Result>
{
    public async Task<Result> Handle(EditProductDetailsCommand command, CancellationToken cancellationToken)
    {
        var productId = Ulid.Parse(command.Id);
        var existentProduct = await dbContext.Products.FirstOrDefaultAsync(ProductSpecifications.ById(productId), cancellationToken);

        if (existentProduct is null)
            return Result.Fail();

        var result = existentProduct.Edit(command.Name, command.Price, command.Stock);

        if (result.IsSuccess is false)
            return Result.Fail(result.Response);

        try
        {
            var productUpdated = (result.Response as Product)!;

            dbContext.Update(productUpdated);

            var success = await dbContext.SaveChangesAsync(cancellationToken) > 0;

            if (!success)
                return Result.Fail(new ProductNotUpdatedError());
        }
        catch (Exception)
        {
            return Result.Fail(new ProductNotUpdatedError());
        }

        return Result.Success("The product has been updated successfully!");
    }
}