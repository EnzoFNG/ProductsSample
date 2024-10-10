using Microsoft.EntityFrameworkCore;
using ProductsSample.Abstractions.Handlers;
using ProductsSample.Abstractions.Primitives;
using ProductsSample.Domain.Products;
using ProductsSample.Domain.Products.Errors;
using ProductsSample.Infrastructure.Context;

namespace ProductsSample.Application.Products.AddNew;

public sealed class AddNewProductCommandHandler(ProductDbContext dbContext) : ICommandHandler<AddNewProductCommand, Result>
{
    public async Task<Result> Handle(AddNewProductCommand command, CancellationToken cancellationToken)
    {
        var existentProduct = await dbContext.Products
            .FirstOrDefaultAsync(ProductSpecifications.ByExactName(command.Name), cancellationToken);

        if (existentProduct is not null)
            return Result.Fail(new ProductAlreadyExistsError());

        var result = Product.Create(command.Name, command.Price, command.Stock);

        if (result.IsSuccess is false)
            return Result.Fail(result.Response);

        var newProduct = (result.Response as Product)!;

        await dbContext.Products.AddAsync(newProduct, cancellationToken);

        var success = await dbContext.SaveChangesAsync(cancellationToken) > 0;

        if (!success)
            return Result.Fail(new ProductNotAddedError());

        return Result.Success("The product has been added successfully!");
    }
}